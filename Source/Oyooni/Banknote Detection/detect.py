import torch
from torchvision import transforms
from PIL import Image
from models.yolo import Model

# Detection method takes image path and returns list of detections
# where each detection is list of [class, confidence score]


@torch.no_grad()
def detect(image_path):
    input = get_input(image_path)
    model = get_model()
    results = get_results(input, model)
    results = map_results(results)
    return results

# getting input tensor from image path


def get_input(image_path):
    transform = transforms.Compose([
        transforms.Resize((416, 416)),
        transforms.ToTensor()
    ])
    img = Image.open(image_path)
    input = transform(img)
    input = input.unsqueeze(0)
    return input

# loading the model


def get_model():
    device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
    model = Model('files/yolov5s.yaml', ch=3, nc=9)
    state_dict = torch.load(
        'files/banknotes.pt', map_location=device)['model'].float().state_dict()  # to FP32
    model.load_state_dict(state_dict, strict=False)  # load
    model.fuse()
    # Adding Non-max Suppression Layer, setting confidence = 0.7
    model.nms(0.7)
    return model

# getting results


def get_results(input, model):
    output = model(input)
    result = []
    for i, det in enumerate(output):
        for *xyxy, conf, cls in reversed(det):
            res = []
            res.append(int(cls.item()))
            res.append(round(conf.item(), 2))
            result.append(res)
    return result

# mapping results from indices to class names


def map_results(results):
    with open('files/classes.txt') as f:
        classes = f.readlines()
    classes = [x.strip() for x in classes]
    for r in results:
        r[0] = classes[r[0]]
    return results
