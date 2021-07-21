from flask import Flask, request
from flask_restful import Resource, Api
from test import get_model, getTextFromImage
from testDocument import getText

app = Flask(__name__)
api = Api(app)

net, refine_net = get_model()


class TextRecognizerService(Resource):
    def post(self):
        try:
            json = request.get_json()
            image_path = json["ImagePath"]
            isDocument = bool(json["IsDocument"])
            if not isDocument:
                brand_name, text = getTextFromImage(
                    image_path, net, refine_net)

                return {
                    "brand_name": brand_name,
                    "text": text
                }, 200

            else:
                return {
                    "text": getText(image_path)
                }, 200

        except Exception as e:
            return {
                'message': e
            }, 501


api.add_resource(TextRecognizerService, "/recognize-text")

if __name__ == "__main__":
    port = 5006
    app.run(port=port)
