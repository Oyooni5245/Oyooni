from flask import Flask, request
from flask_restful import Resource, Api
from test import get_models, getTextFromImage
from testDocument import getText
from time import time

app = Flask(__name__)
api = Api(app)

net, refine_net = get_models()


class TextRecognizerService(Resource):
    def post(self):
        try:
            json = request.get_json()
            image_path = json["ImagePath"]
            isDocument = bool(json["IsDocument"])
            if isDocument == False:
                start = time()
                brand_name, texts, language = getTextFromImage(
                    image_path, net, refine_net)
                end = time()

                return {
                    "brand_name": brand_name,
                    "texts": texts,
                    "language": language,
                    "inference_time": end - start
                }, 200

            else:
                text, language = getText(image_path, 'fullDocument.json')
                return {
                    "text": text,
                    "language": language
                }, 200

        except Exception as e:
            return {
                'message': e
            }, 501


api.add_resource(TextRecognizerService, "/recognize-text")

if __name__ == "__main__":
    port = 5006
    app.run(debug=True, port=port)
