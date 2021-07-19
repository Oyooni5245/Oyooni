import cv2
import pytesseract

pytesseract.pytesseract.tesseract_cmd = r'C:/Program Files/Tesseract-OCR/tesseract.exe'


def preprocessImage(imgUrl):
    img = cv2.imread(imgUrl)
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    gray = cv2.bitwise_not(gray)
    # threshold the image, setting all foreground pixels to
    # 255 and all background pixels to 0
    thresh = cv2.threshold(gray, 0, 255,
                           cv2.THRESH_BINARY | cv2.THRESH_OTSU)[1]
    # invert black/white
    inv = cv2.bitwise_not(thresh)
    return inv


def getText(imageUrl):
    processedImage = preprocessImage(imageUrl)
    custom_config = r'--oem 3 --psm 6'
    text = pytesseract.image_to_string(
        processedImage, config=custom_config, lang='ara+eng')
    fullTextList = []
    fullTextList.append(text)
    words = '+'.join([str(x) for x in fullTextList])
    return words
