import model

import pickle
import warnings

warnings.filterwarnings("ignore")

import tensorflow as tf


def load_image(image_path):
    img = tf.io.read_file(image_path)
    img = tf.image.decode_jpeg(img, channels=3)
    img = tf.image.resize(img, (299, 299))
    img = tf.keras.applications.inception_v3.preprocess_input(img)
    return img, image_path

def load_tokenizer():
    with open('tokenizer.pkl', 'rb') as f:
        tokenizer = pickle.load(f)
    return tokenizer

def create_masks_decoder(tar):
    look_ahead_mask = model.create_look_ahead_mask(tf.shape(tar)[1])
    dec_target_padding_mask = model.create_padding_mask(tar)
    combined_mask = tf.maximum(dec_target_padding_mask, look_ahead_mask)
    return combined_mask
