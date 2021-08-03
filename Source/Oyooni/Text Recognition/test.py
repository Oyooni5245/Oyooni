"""  
Copyright (c) 2019-present NAVER Corp.
MIT License
"""

from collections import OrderedDict

import config
from craft import CRAFT
import codecs
import imgproc
import craft_utils
import cv2
import numpy as np
import pytesseract
from torch.autograd import Variable
import torch.backends.cudnn as cudnn
import torch
import os
import time
import regex as re
os.environ["KMP_DUPLICATE_LIB_OK"] = "TRUE"
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


def hasNumbers(inputString):
    return any(char.isdigit() for char in inputString)


def getFullText(imgs, path, lang):
    custom_config = r'--oem 3 --psm 6'
    fullText = ""
    for img in imgs:
        currentImage = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        currentImage = cv2.cvtColor(currentImage, cv2.COLOR_BGR2RGB)

        gray = cv2.cvtColor(currentImage, cv2.COLOR_BGR2GRAY)
        gray2 = cv2.bitwise_not(gray)
        thresh = cv2.threshold(
            gray2, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)[1]
        mylang = ''
        anotherText = ''
        if(lang == 'en'):
            mylang = 'eng'
        else:
            mylang = 'ara+eng'

        currentText = pytesseract.image_to_string(
            thresh, config=custom_config, lang=mylang)
        if(not currentText.strip()):
            currentText = pytesseract.image_to_string(
                currentImage, config=custom_config, lang=mylang)
        if hasNumbers(currentText):
            if mylang == 'ara':
                sLang = 'eng'
                anotherText = pytesseract.image_to_string(
                    thresh, config=custom_config, lang=sLang)

            elif mylang == 'ara+eng':
                sLang = 'eng'
                anotherText = pytesseract.image_to_string(
                    thresh, config=custom_config, lang='eng')
            if hasNumbers(anotherText):
                sLang = 'ara+eng'
                anotherText = pytesseract.image_to_string(
                    thresh, config=custom_config, lang=sLang)
            if hasNumbers(anotherText) == False and len(anotherText.strip()) > 0:
                currentText = anotherText
        currentText = currentText.strip()
        if(currentText):
            fullText += " "+currentText
    fullTextList = []
    fullTextList.append(fullText)
    arText = ''
    enText = ''
    for txt in fullText.split(' '):
        if checkTextLang(txt) == 'en':
            enText += txt+" "

        else:
            arText += txt+" "
    textDict = dict()
    textDict['ar'] = arText
    textDict['en'] = enText

    with codecs.open(path, 'w', 'utf-8') as outfile:
        outfile.write('+'.join([str(x) for x in fullTextList]))
    with codecs.open(path, 'r', 'utf-8') as outfile:
        textDict['full_text'] = outfile.readlines()
    return textDict


def getBrandText(image, path):
    textList = []
    custom_config = r'--oem 3 --psm 6'
    currentImage = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    currentText = pytesseract.image_to_string(
        currentImage, config=custom_config, lang='eng+ara')
    currentText = currentText.strip()
    if(currentText):
        textList.append(currentText)

    with codecs.open(path, 'w', 'utf-8') as outfile:
        outfile.write('+'.join([str(x) for x in textList]))
    with codecs.open(path, 'r', 'utf-8') as readFile:
        brand_name = readFile.readline()
    return brand_name


def ConvertPositionsToImages(boxPositions, image):
    croppedTextBoxImages = dict()
    maxArea = 1
    for pos in (boxPositions):
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


def checkTextLang(text):
    x = re.search('[a-zA-Z]', text)
    if x:
        return 'en'
    else:
        return 'ar'


def checkLang(img, alreadyPreprocessedImage=False):
    custom_config = r'--oem 3 --psm 6'

    if alreadyPreprocessedImage == True:
        text = pytesseract.image_to_string(
            img, config=custom_config, lang='eng+ara')
        x = re.search('[a-zA-Z]', text)
        if x:
            return 'en'
        else:
            return 'ar'

    currentImage = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
    currentImage = cv2.cvtColor(currentImage, cv2.COLOR_BGR2RGB)
    gray = cv2.cvtColor(currentImage, cv2.COLOR_BGR2GRAY)
    gray2 = cv2.bitwise_not(gray)
    thresh = cv2.threshold(
        gray2, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)[1]
    text = pytesseract.image_to_string(
        thresh, config=custom_config, lang='eng+ara')
    x = re.search('[a-zA-Z]', text)
    if x:
        return 'en'
    else:
        return 'ar'


def classifyBoxes(boxPositions):
    classifiedBoxes = {}
    firstBox = boxPositions[0]
    classifiedBoxes[firstBox[1]] = []
    classifiedBoxes[firstBox[1]].append(firstBox)
    flag = True
    sFlag = False
    for pos in boxPositions:
        if(flag):
            flag = False

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
    if(lang == 'en'):
        for key in classifiedBoxes.keys():
            keyList = classifiedBoxes[key]
            keyList = sorted(keyList, key=lambda x: x[3], reverse=False)
            classifiedBoxes[key] = keyList

    else:
        for key in classifiedBoxes.keys():
            keyList = classifiedBoxes[key]
            keyList = sorted(keyList, key=lambda x: x[3], reverse=True)
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

    img_resized, target_ratio, _ = imgproc.resize_aspect_ratio(
        image, config.canvas_size, interpolation=cv2.INTER_LINEAR, mag_ratio=config.mag_ratio)
    ratio_h = ratio_w = 1 / target_ratio

    x = imgproc.normalizeMeanVariance(img_resized)
    x = torch.from_numpy(x).permute(2, 0, 1)
    x = Variable(x.unsqueeze(0))
    if cuda:
        x = x.cuda()

    with torch.no_grad():
        y, feature = net(x)

    score_text = y[0, :, :, 0].cpu().data.numpy()
    score_link = y[0, :, :, 1].cpu().data.numpy()

    if refine_net is not None:
        with torch.no_grad():
            y_refiner = refine_net(y, feature)
        score_link = y_refiner[0, :, :, 0].cpu().data.numpy()

    t0 = time.time() - t0
    t1 = time.time()

    boxes, polys = craft_utils.getDetBoxes(
        score_text, score_link, text_threshold, link_threshold, low_text, poly)

    boxes = craft_utils.adjustResultCoordinates(boxes, ratio_w, ratio_h)
    polys = craft_utils.adjustResultCoordinates(polys, ratio_w, ratio_h)
    for k in range(len(polys)):
        if polys[k] is None:
            polys[k] = boxes[k]

    t1 = time.time() - t1

    render_img = score_text.copy()
    render_img = np.hstack((render_img, score_link))
    ret_score_text = imgproc.cvt2HeatmapImg(render_img)

    return boxes, polys, ret_score_text


def get_models():
    net = CRAFT()

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

    refine_net = None
    if config.refine:
        from refinenet import RefineNet
        refine_net = RefineNet()
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
    image = imgproc.loadImage(image_path)

    bboxes, _, _ = test_net(
        net, image, config.text_threshold, config.link_threshold, config.low_text, config.cuda, config.poly, refine_net)

    boxPositions = []

    for box in (bboxes):
        boxPolys = np.array(box).astype(np.int32).reshape((-1))
        boxPolys[boxPolys < 0] = 0
        minY, maxY, minX, maxX = min(boxPolys[1], boxPolys[3], boxPolys[5], boxPolys[7]), max(boxPolys[1], boxPolys[3], boxPolys[5], boxPolys[7]), min(
            boxPolys[0], boxPolys[2], boxPolys[4], boxPolys[6]), max(boxPolys[0], boxPolys[2], boxPolys[4], boxPolys[6])

        boxPositions.append((minY, maxY, minX, maxX))

    boxPositions = sorted(boxPositions, key=lambda x: x[1])
    brand_name = ""
    text = ""
    language = "en"
    if len(boxPositions) > 0:

        croppedTextBoxImages, maxArea = ConvertPositionsToImages(
            boxPositions, image)
        brand_name = getBrandText(
            croppedTextBoxImages[maxArea], 'brand-name-temp.json')

        Mainlang = checkLang(croppedTextBoxImages[maxArea])
        language = Mainlang
        classifiedBoxes = classifyBoxes(boxPositions)
        classifiedBoxes = sortClassifiedBoxes(classifiedBoxes, Mainlang)
        boxPositions = convertDictToList(classifiedBoxes)
        croppedTextBoxImages, maxArea = ConvertPositionsToImages(
            boxPositions, image)

        text = getFullText(convPosToImages(
            boxPositions, image), language, 'temp.json')

    return brand_name, text, language
