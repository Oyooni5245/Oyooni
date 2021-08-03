import cv2
import pytesseract
pytesseract.pytesseract.tesseract_cmd = r'C:/Program Files/Tesseract-OCR/tesseract.exe'
#image path
imgUrl=r'C:/Users/abdq9/Desktop/test.jpg'

img=cv2.imread(imgUrl)

custom_config = r'--oem 3 --psm 6'
text = pytesseract.image_to_string(img,config=custom_config, lang= 'ara+eng')