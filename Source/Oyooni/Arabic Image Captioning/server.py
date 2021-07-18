from flask_restful import Resource, Api
from flask import Flask, request
from test import caption_image
import warnings
import model
import time

warnings.filterwarnings("ignore")

tokenizer = model.tokenizer
transformer = model.create_model()

app = Flask(__name__)
api = Api(app)


class ArabicImageCaptionerService(Resource):
    def post(self):
        try:
            image_path = request.get_json()["ImagePath"]
            start_time = time.time()
            caption = caption_image(image_path, tokenizer, transformer)
            captioning_time = time.time() - start_time
            return {
                "caption": caption,
                "captioning_speed": captioning_time
            }, 200

        except Exception as e:
            return {
                'message': e
            }, 501


api.add_resource(ArabicImageCaptionerService, "/caption-image")

if __name__ == "__main__":
    port = 5005
    app.run(port=port)
