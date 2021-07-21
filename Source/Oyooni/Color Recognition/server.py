from flask import Flask, request
from flask_restful import Resource, Api
from color_classification_image import predict_color

app = Flask(__name__)
api = Api(app)

colorToIndex = {
    'black': 0,
    'white': 1,
    'red': 2,
    'orange': 3,
    'green': 4,
    'blue': 5,
    'yellow': 6,
}

class ColorRecognizer(Resource):
    def post(self):
        image_path = request.get_json()['ImagePath']
        prediction = predict_color(image_path)
        return {"recognizedColor": colorToIndex[prediction]}, 200


api.add_resource(ColorRecognizer, "/recognize-color")

if __name__ == "__main__":
    port = 5004
    app.run(port=port)
