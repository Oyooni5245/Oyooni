from flask import Flask, request
from flask_restful import Resource, Api
from keras.models import load_model
import base64
import io
from imageio import imread
import cv2
import numpy as np

app = Flask(__name__)
api = Api(app)
model = load_model("model.h5")


def base64_to_image(base64_string):
    return imread(io.BytesIO(base64.b64decode(base64_string)))


class DigitRecognitionService(Resource):
    def post(self):
        # Get the base64 image data
        image = base64_to_image(request.get_json()["Base64Data"])

        # Change the image to grayscale if needed
        if (len(image.shape) > 2):
            image = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

        # Resize the image to (28, 28) and reshape it to (1, 28, 28, 1) and make it of type float
        image = cv2.resize(image, (28, 28)).reshape(
            (1, 28, 28, 1)).astype('float32')

        recognizedDigit = np.argmax(model.predict(image))

        return {"recognizedDigit": str(recognizedDigit)}, 200


api.add_resource(DigitRecognitionService, "/recognize-digit")

if __name__ == "__main__":
    port = 5000
    app.run(debug=True, port=port)
