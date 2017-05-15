import string

def isNullOrEmpty(str):
    return str == None or str == ''

def isNullOrWhitespace(str):
    if isNullOrEmpty(str):
        return True
    else:
        result = str.strip()
        return isNullOrEmpty(result) 
