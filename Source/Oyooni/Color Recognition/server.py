from flask import Flask, request
from flask_restful import Resource, Api
from detect import detectColors, get_color_dict

app = Flask(__name__)
api = Api(app)

colorDict = get_color_dict()

class ColorRecognizer(Resource):
    def post(self):
        image_path = request.get_json()['ImagePath']
        detectedColor = detectColors(image_path, colorDict)
        return {"recognizedColor": detectedColor}, 200


api.add_resource(ColorRecognizer, "/recognize-color")

if __name__ == "__main__":
    port = 5004
    app.run(port=port)
