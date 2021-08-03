from flask import Flask, request
from flask_restful import Resource, Api
from detect import get_model, get_input, get_results, map_results
from utils.mapping import map_to_server_type

app = Flask(__name__)
api = Api(app)

model = get_model()


class BanknoteDetectorService(Resource):
    def post(self):
        try:
            image_path = request.get_json()["ImagePath"]
            input = get_input(image_path)
            model = get_model()
            results = get_results(input, model)
            results = map_results(results)
            return {
                "result": map_to_server_type(results[0][0] if len(results) > 0 else "Undefinded")
            }, 200
        except Exception as e:
            return {
                'message': e
            }, 501


api.add_resource(BanknoteDetectorService, "/detect-banknote")

if __name__ == "__main__":
    port = 5003
    app.run(port=port)
