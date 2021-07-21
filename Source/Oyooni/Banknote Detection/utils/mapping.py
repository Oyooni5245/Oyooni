def map_to_server_type(predicted_class):
    server_type = ''

    if (predicted_class == 'two_thousand'):
        server_type = 7
    elif (predicted_class == 'fifty'):
        server_type = 1
    elif (predicted_class == 'one_hundred'):
        server_type = 2
    elif (predicted_class == 'two_hundred'):
        server_type = 9
    elif (predicted_class == 'five_hundred_new'):
        server_type = 4
    elif (predicted_class == 'five_hundred_old'):
        server_type = 3
    elif (predicted_class == 'one_thousand_new'):
        server_type = 6
    elif (predicted_class == 'one_thousand_old'):
        server_type = 5
    elif (predicted_class == 'five_thousand'):
        server_type = 8
    else:
        server_type = 10

    return server_type
