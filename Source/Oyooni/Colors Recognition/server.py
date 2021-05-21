from dominant_color_detection import detect_colors
import dominant_color_detection as dcd
from scipy.spatial import KDTree
from webcolors import CSS3_HEX_TO_NAMES, hex_to_rgb
from flask import Flask, request
from flask_restful import Resource, Api
import base64
import io
from imageio import imread


app = Flask(__name__)
api = Api(app)

css3_db = CSS3_HEX_TO_NAMES
names = []
rgb_values = []
for color_hex, color_name in css3_db.items():
    names.append(color_name)
    rgb_values.append(hex_to_rgb(color_hex))

kdt_db = KDTree(rgb_values)


def hex_to_color_name(hex_value):
    distance, index = kdt_db.query(hex_to_rgb(hex_value))
    return names[index]


def base64_to_image(base64_string):
    return imread(io.BytesIO(base64.b64decode(base64_string)))


class ColorDetector(Resource):
    def post(self):
        json = request.get_json()
        imageStream = io.BytesIO(base64.b64decode(json["Base64Data"]))
        k = 3

        if ('K' in json):
            k = int(json["K"])

        colors, ratios = dcd.detect_colors(imageStream, k)

        response = dict()

        for i in range(len(colors)):
            colorName = hex_to_color_name(colors[i])
            response[colorName] = ratios[i]

        return response, 200


api.add_resource(ColorDetector, "/recognize-colors")

if __name__ == "__main__":
    port = 5001
    app.run(debug=True, port=port)
