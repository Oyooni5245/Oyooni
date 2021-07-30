"""  
Copyright (c) 2019-present NAVER Corp.
MIT License
"""

# -*- coding: utf-8 -*-
from collections import OrderedDict

import googletrans
import config
from craft import CRAFT
from googletrans import Translator
import codecs
import zipfile
import file_utils
import imgproc
import craft_utils
from skimage import io
import cv2
import json
from PIL import Image
import numpy as np
import pytesseract
from torch.autograd import Variable
import torch.backends.cudnn as cudnn
import torch.nn as nn
import torch
import sys
import os
import time
import argparse
os.environ["KMP_DUPLICATE_LIB_OK"] = "TRUE"  # used to solve an error
pytesseract.pytesseract.tesseract_cmd = r'C:\Program Files\Tesseract-OCR\tesseract.exe'


def copyStateDict(state_dict):
    if list(state_dict.keys())[0].startswith("module"):
        start_idx = 1
    else:
        start_idx = 0
    new_state_dict = OrderedDict()
    for k, v in state_dict.items():
        name = ".".join(k.split(".")[start_idx:])
        new_state_dict[name] = v
    return new_state_dict


def str2bool(v):
    return v.lower() in ("yes", "y", "true", "t", "1")


# parser = argparse.ArgumentParser(description='CRAFT Text Detection')
# parser.add_argument('--trained_model', default='craft_mlt_25k.pth',
#                     type=str, help='pretrained model')
# parser.add_argument('--text_threshold', default=0.7,
#                     type=float, help='text confidence threshold')
# parser.add_argument('--low_text', default=0.4, type=float,
#                     help='text low-bound score')
# parser.add_argument('--link_threshold', default=0.4,
#                     type=float, help='link confidence threshold')
# parser.add_argument('--cuda', default=False, type=str2bool,
#                     help='Use cuda for inference')
# parser.add_argument('--canvas_size', default=1280,
#                     type=int, help='image size for inference')
# parser.add_argument('--mag_ratio', default=1.5, type=float,
#                     help='image magnification ratio')
# parser.add_argument('--poly', default=False,
#                     action='store_true', help='enable polygon type')
# parser.add_argument('--show_time', default=False,
#                     action='store_true', help='show processing time')
# parser.add_argument('--test_folder', default='Test images/',
#                     type=str, help='folder path to input images')
# parser.add_argument('--refine', default=False,
#                     action='store_true', help='enable link refiner')
# parser.add_argument('--refiner_model', default='craft_refiner_CTW1500.pth',
#                     type=str, help='pretrained refiner model')

# args = parser.parse_args()


""" For test images in a folder """
# image_list, _, _ = file_utils.get_files(config.test_folder)

# result_folder = 'C:/Users/Sami/Desktop/results'
# if not os.path.isdir(result_folder):
# os.mkdir(result_folder)


"""
def getFullText(imgs,path):
    custom_config = r'--oem 3 --psm 6'
    fullText = ""
    for imgKey in imgs.keys():
        currentImage=cv2.cvtColor(imgs[imgKey], cv2.COLOR_BGR2RGB)
        fullText +=" "+pytesseract.image_to_string(currentImage, config=custom_config, lang='ara')
    fullTextList=[]
    fullTextList.append(fullText)
    with codecs.open(path, 'w','utf-8') as outfile:
        outfile.write('+'.join([str(x) for x in fullTextList]));
"""


def getFullText(imgs, language, path):
    custom_config = r'--oem 3 --psm 6'
    fullText = ""
    for img in imgs:
        currentImage = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        currentText = pytesseract.image_to_string(
            currentImage, config=custom_config, lang=('eng' if language == 'en' else 'ara'))
        currentText = currentText.strip()
        if(currentText):
            fullText += " "+currentText
    fullTextList = []
    fullTextList.append(fullText)
    with codecs.open(path, 'w', 'utf-8') as outfile:
        outfile.write('+'.join([str(x) for x in fullTextList]))
    with codecs.open(path, 'r', 'utf-8') as file:
        words = file.readlines()
    return words


def getBrandText(image):
    textList = []
    custom_config = r'--oem 3 --psm 6'
    currentImage = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    currentText = pytesseract.image_to_string(
        currentImage, config=custom_config, lang='eng+ara')
    currentText = currentText.strip()
    if(currentText):
        textList.append(currentText)

    brandName = '+'.join([str(x) for x in textList]).strip()
    return brandName


def ConvertPositionsToImages(boxPositions, image):
    croppedTextBoxImages = dict()
    # get the box with the highest area.
    maxArea = 1
    for pos in (boxPositions):
        # print("Pos 0: ", pos[0] ," Pos 1: ", pos[1] , " Pos 2: " , pos[2] , " Pos 3: ", pos[3])
        area = (pos[1]-pos[0])*(pos[3]-pos[2])
        if area in croppedTextBoxImages.keys():
            area += 1
        if area > maxArea:
            maxArea = area
        croppedTextBoxImages[area] = image[pos[0]:pos[1], pos[2]: pos[3]]
    return croppedTextBoxImages, maxArea


def convPosToImages(boxpositions, image):
    resList = []
    for pos in boxpositions:
        resList.append(image[pos[0]:pos[1], pos[2]: pos[3]])
    return resList


def checkLang(img):
    try:
        custom_config = r'--oem 3 --psm 6'
        currentImage = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        text = pytesseract.image_to_string(
            currentImage, config=custom_config, lang='eng+ara')
        # print("\n", text, "\n")
        detector = Translator()
        dec_lan = detector.detect(text)
        return dec_lan.lang
    except:
        return "en"


def classifyBoxes(boxPositions):
    classifiedBoxes = {}
    firstBox = boxPositions[0]
    classifiedBoxes[firstBox[1]] = []
    classifiedBoxes[firstBox[1]].append(firstBox)
    # print(classifiedBoxes.keys())
    flag = True
    sFlag = False
    for pos in boxPositions:
        if(flag):
            flag = False
            # print(classifiedBoxes.copy().keys())
        else:
            myval = pos[1]
            for key in classifiedBoxes.copy().keys():
                if myval in range(key, key+20, 1):
                    sFlag = False
                    classifiedBoxes[key].append(pos)
                    break
                else:
                    sFlag = True
        if sFlag:
            classifiedBoxes[myval] = []
            classifiedBoxes[myval].append(pos)
    return classifiedBoxes


def sortClassifiedBoxes(classifiedBoxes, lang):
    if(lang == 'ar'):
        for key in classifiedBoxes.keys():
            keyList = classifiedBoxes[key]
            keyList = sorted(keyList, key=lambda x: x[3], reverse=True)
            classifiedBoxes[key] = keyList

    else:
        for key in classifiedBoxes.keys():
            keyList = classifiedBoxes[key]
            keyList = sorted(keyList, key=lambda x: x[3], reverse=False)
            classifiedBoxes[key] = keyList
    return classifiedBoxes


def convertDictToList(res):
    resList = []
    for key in res.keys():
        for element in res[key]:
            resList.append(element)
    return resList


def test_net(net, image, text_threshold, link_threshold, low_text, cuda, poly, refine_net=None):
    t0 = time.time()

    # resize
    img_resized, target_ratio, size_heatmap = imgproc.resize_aspect_ratio(
        image, config.canvas_size, interpolation=cv2.INTER_LINEAR, mag_ratio=config.mag_ratio)
    ratio_h = ratio_w = 1 / target_ratio

    # preprocessing
    x = imgproc.normalizeMeanVariance(img_resized)
    x = torch.from_numpy(x).permute(2, 0, 1)    # [h, w, c] to [c, h, w]
    x = Variable(x.unsqueeze(0))                # [c, h, w] to [b, c, h, w]
    if cuda:
        x = x.cuda()

    # forward pass
    with torch.no_grad():
        y, feature = net(x)

    # make score and link map
    score_text = y[0, :, :, 0].cpu().data.numpy()
    score_link = y[0, :, :, 1].cpu().data.numpy()

    # refine link
    if refine_net is not None:
       # print('in refien net')
        with torch.no_grad():
            y_refiner = refine_net(y, feature)
        score_link = y_refiner[0, :, :, 0].cpu().data.numpy()

    t0 = time.time() - t0
    t1 = time.time()

    # Post-processing
    boxes, polys = craft_utils.getDetBoxes(
        score_text, score_link, text_threshold, link_threshold, low_text, poly)

    # coordinate adjustment
    boxes = craft_utils.adjustResultCoordinates(boxes, ratio_w, ratio_h)
    polys = craft_utils.adjustResultCoordinates(polys, ratio_w, ratio_h)
    for k in range(len(polys)):
        if polys[k] is None:
            polys[k] = boxes[k]

    t1 = time.time() - t1

    # render results (optional)
    render_img = score_text.copy()
    render_img = np.hstack((render_img, score_link))
    ret_score_text = imgproc.cvt2HeatmapImg(render_img)

    # if show_time:
    # print("\ninfer/postproc time : {:.3f}/{:.3f}".format(t0, t1))

    return boxes, polys, ret_score_text


def get_model():
    # load net
    net = CRAFT()     # initialize

    # print('Loading weights from checkpoint (' + trained_model + ')')
    if config.cuda:
        net.load_state_dict(copyStateDict(torch.load(config.trained_model)))
    else:
        net.load_state_dict(copyStateDict(torch.load(
            config.trained_model, map_location=torch.device('cpu'))))

    if config.cuda:
        net = net.cuda()
        net = torch.nn.DataParallel(net)
        cudnn.benchmark = False

    net.eval()

    # LinkRefiner
    refine_net = None
    if config.refine:
        from refinenet import RefineNet
        refine_net = RefineNet()
        # print('Loading weights of refiner from checkpoint (' + refiner_model + ')')
        if config.cuda:
            refine_net.load_state_dict(
                copyStateDict(torch.load(config.refiner_model)))
            refine_net = refine_net.cuda()
            refine_net = torch.nn.DataParallel(refine_net)
        else:
            refine_net.load_state_dict(copyStateDict(torch.load(
                config.refiner_model, map_location=torch.device('cpu'))))

        refine_net.eval()
        config.poly = True

    return net, refine_net


def getTextFromImage(image_path, net, refine_net=None):
    # readImageTime = time.time()
    # print("Test image {:d}/{:d}: {:s}".format(k+1,
    # len(image_list), image_path), end='\r')
    image = imgproc.loadImage(image_path)
    # print("read image time : {}s".format(time.time() - readImageTime))

    # testNetTime = time.time()
    bboxes, polys, score_text = test_net(
        net, image, config.text_threshold, config.link_threshold, config.low_text, config.cuda, config.poly, refine_net)
    # print("test net time : {}s".format(time.time() - testNetTime))
    boxPositions = []

    # gettCorTime = time.time()
    for box in (bboxes):
        # Get coordtinates for bounding boxes here
        boxPolys = np.array(box).astype(np.int32).reshape((-1))
        boxPolys[boxPolys < 0] = 0  # Negative values will be set to zero
        #
        minY, maxY, minX, maxX = min(boxPolys[1], boxPolys[3], boxPolys[5], boxPolys[7]), max(boxPolys[1], boxPolys[3], boxPolys[5], boxPolys[7]), min(
            boxPolys[0], boxPolys[2], boxPolys[4], boxPolys[6]), max(boxPolys[0], boxPolys[2], boxPolys[4], boxPolys[6])
        # print("bbox array:", poly)
        boxPositions.append((minY, maxY, minX, maxX))

    boxPositions = sorted(boxPositions, key=lambda x: x[1])
    # print("get coords time : {}s".format(time.time() - gettCorTime))
    # mymethodTime = time.time()
    brand_name = ""
    text = ""
    language = "en"
    if len(boxPositions) > 0:

        # Extract the boxpositions as images
        croppedTextBoxImages, maxArea = ConvertPositionsToImages(
            boxPositions, image)
        # print("my method time: {}s".format(time.time() - mymethodTime))
        brand_name = getBrandText(croppedTextBoxImages[maxArea])
        Mainlang = checkLang(croppedTextBoxImages[maxArea])
        print("Main Language:", Mainlang)
        language = Mainlang
        classifiedBoxes = classifyBoxes(boxPositions)
        classifiedBoxes = sortClassifiedBoxes(classifiedBoxes, Mainlang)
        boxPositions = convertDictToList(classifiedBoxes)
        # print(classifiedBoxes)
        # print("MinY MaxY MinX MaxX")
        # for pos in boxPositions:
        #print(pos[0], ",", pos[1], ",", pos[2], ",", pos[3])
        # croppedTextBoxImages, maxArea = ConvertPositionsToImages(
        #     boxPositions)
        # img = cv2.cvtColor(
        #     croppedTextBoxImages[maxArea], cv2.COLOR_BGR2RGB)
        # cv2.imshow('image', img)
        # cv2.waitKey(0)
        # cv2.imwrite('croppedimage.jpg', img)
        # ocrStartT = time.time()
        text = getFullText(convPosToImages(
            boxPositions, image), language, 'temp.json')
        # print("fulltext time : {}s".format(time.time() - ocrStartT))

        # save score text

        # filename, file_ext = os.path.splitext(os.path.basename(image_path))
        # mask_file = result_folder + "/res_" + filename + '_mask.jpg'
        # cv2.imwrite(mask_file, score_text)

        # file_utils.saveResult(
        #     image_path, image[:, :, ::-1], polys, dirname=result_folder)

    return brand_name, text, language


# if __name__ == '__main__':

#     net, refine_net = get_model()

#     # t = time.time()

#     # load data
#     for _, image_path in enumerate(image_list):
#         brand_name, text = getTextFromImage(image_path, net, refine_net)

#         print("Brand Name", brand_name)
#         print("Text", text)

    # print("elapsed time : {}s".format(time.time() - t))
