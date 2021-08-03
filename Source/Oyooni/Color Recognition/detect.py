import binascii
from PIL import Image
import numpy as np
import scipy
import scipy.misc
import scipy.cluster

def get_color_dict():
    colorDict = dict()
    colorDict['أسود'] = [0, 0, 0]
    colorDict['رمادي'] = [127, 127, 127]
    colorDict['خمري'] = [136, 0, 21]
    colorDict['أحمر'] = [237, 28, 36]
    colorDict['برتقالي'] = [255, 127, 39]
    colorDict['أصفر'] = [255, 242, 0]
    colorDict['أخضر'] = [34, 177, 76]
    colorDict['ازرق'] = [203, 228, 253]
    colorDict['ازرق داكن'] = [0, 0, 128]
    colorDict['أزرق ملكي'] = [65, 105, 225]
    colorDict['احمر غامق'] = [139, 0, 0]
    colorDict['أخضرغامق'] = [0, 100, 0]
    colorDict['زيتوني'] = [85, 107, 47]
    colorDict['أزرق غامق'] = [0, 162, 232]
    colorDict['بنفسجي'] = [63, 72, 204]
    colorDict['أبيض'] = [255, 255, 255]
    colorDict['رمادي فاتح'] = [195, 195, 195]
    colorDict['زهري'] = [230, 39, 115]
    colorDict['فوشيا'] = [255, 0, 255]
    colorDict['زهري غامق'] = [255, 20, 147]
    return colorDict


def closest(colors, color):
    colors = np.array(colors)
    color = np.array(color)
    distances = np.sqrt(np.sum((colors-color)**2, axis=1))
    index_of_smallest = np.where(distances == np.amin(distances))
    smallest_distance = colors[index_of_smallest]
    return smallest_distance[0]


def detectColors(path, colorDict):

    NUM_CLUSTERS = 5

    im = Image.open(path)
    im = im.resize((150, 150))      # optional, to reduce time
    ar = np.asarray(im)
    shape = ar.shape
    ar = ar.reshape(scipy.product(shape[:2]), shape[2]).astype(float)

    codes, _ = scipy.cluster.vq.kmeans(ar, NUM_CLUSTERS)

    vecs, _ = scipy.cluster.vq.vq(ar, codes)         # assign codes
    counts, _ = scipy.histogram(vecs, len(codes))    # count occurrences

    index_max = scipy.argmax(counts)                    # find most frequent
    peak = codes[index_max]
    colour = binascii.hexlify(bytearray(int(c) for c in peak)).decode('ascii')
    rgb = list(int(colour[i:i+2], 16) for i in (0, 2, 4))
    detectedColor = closest(list(colorDict.values()), rgb)
    detectedColor = list(detectedColor)
    outputColor = 'لم يتم التعرف على اللون'
    for key in colorDict.keys():
        if colorDict[key] == detectedColor:
            outputColor = key
    return outputColor
