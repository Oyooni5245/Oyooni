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
            if isDocument == False:
                brand_name, text = getTextFromImage(
                    image_path, net, refine_net)

                # print("Predictions:\n\tBrand Name:",
                #       brand_name, "\n\tText:", text, "\n")

                return {
                    "brand_name": brand_name,
                    "text": text
                }, 200

            else:
                text = getText(image_path)
                return {
                    "text": text
                }, 200

        except Exception as e:
            return {
                'message': e
            }, 501


api.add_resource(TextRecognizerService, "/recognize-text")

if __name__ == "__main__":
    port = 5006
    app.run(debug=True, port=port)
