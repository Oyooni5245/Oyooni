from test import checkLang
import codecs
import cv2
import pytesseract
pytesseract.pytesseract.tesseract_cmd = r'C:/Program Files/Tesseract-OCR/tesseract.exe'


def preprocessImage(imgUrl):
    img = cv2.imread(imgUrl)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    gray = cv2.bitwise_not(gray)
    thresh = cv2.threshold(gray, 0, 255,
                           cv2.THRESH_BINARY | cv2.THRESH_OTSU)[1]

    inv = cv2.bitwise_not(thresh)
    return inv


def getText(imageUrl, path):
    processedImage = preprocessImage(imageUrl)
    language = checkLang(processedImage, True)
    myLanguage = ""
    if language == 'en':
        myLanguage = 'eng'
    else:
        myLanguage = 'ara'

    custom_config = r'--oem 3 --psm 6'
    text = pytesseract.image_to_string(
        processedImage, config=custom_config, lang=myLanguage)
    fullTextList = []
    fullTextList.append(text)
    with codecs.open(path, 'w', 'utf-8') as outfile:
        outfile.write('+'.join([str(x) for x in fullTextList]))
    with codecs.open(path, 'r', 'utf-8') as readfile:
        words = readfile.readlines()

    print("After writing to the file in document detection")
    return words, language
