import model
import utils
import warnings
warnings.filterwarnings("ignore")
import tensorflow as tf

tokenizer = model.tokenizer
transformer = model.create_model()

def evaluate(image):
    temp_input = tf.expand_dims(utils.load_image(image)[0], 0)
    img_tensor_val = model.image_features_extract_model(temp_input)
    img_tensor_val = tf.reshape(img_tensor_val, (img_tensor_val.shape[0], -1, img_tensor_val.shape[3]))

    start_token = tokenizer.word_index['<start>']
    end_token = tokenizer.word_index['<end>']

    # decoder input is start token.
    decoder_input = [start_token]
    output = tf.expand_dims(decoder_input, 0)  # tokens
    result = []  # word list

    for i in range(30):
        dec_mask = utils.create_masks_decoder(output)

        # predictions.shape == (batch_size, seq_len, vocab_size)
        predictions, attention_weights = transformer(img_tensor_val, output, False, dec_mask)

        # select the last word from the seq_len dimension
        predictions = predictions[:, -1:, :]  # (batch_size, 1, vocab_size)

        predicted_id = tf.cast(tf.argmax(predictions, axis=-1), tf.int32)
        # return the result if the predicted_id is equal to the end token
        if predicted_id == end_token:
            return result, tf.squeeze(output, axis=0), attention_weights
        # concatentate the predicted_id to the output which is given to the decoder
        # as its input.
        result.append(tokenizer.index_word[int(predicted_id)])
        output = tf.concat([output, predicted_id], axis=-1)

    return result, tf.squeeze(output, axis=0), attention_weights

def test(image_path):
    caption, result, attention_weights = evaluate(image_path)
    final_caption =[]
    for i in range(len(caption)-1):
        if caption[i] !='<unk>' and caption[i] != caption[i+1]:
            final_caption.append(caption[i])
        if i == len(caption)-2 and caption[i] != '<unk>':
            final_caption.append(caption[i+1])
    final_caption = ' '.join(final_caption)
    return final_caption


#Once everything is cached, the below command takes 2 seconds to execute
print(test("test_images/motor.jpg"))

#print(test("test_images/asian-woman.jpg"))
#print(test("test_images/beach2.jpg"))
#print(test("test_images/climbing5.jpg"))
#print(test("test_images/kids-bike.jpg"))
#print(test("test_images/old-woman2.jpg"))
#print(test("test_images/pool3.jpg"))
#print(test("test_images/rugby.jpg"))
#print(test("test_images/snow5.jpg"))
#print(test("test_images/sports.jpeg"))
#print(test("test_images/young-man-climbing.jpg"))
#print(test("test_images/dogs2.jpg"))
#print(test("test_images/dogs4.jpeg"))