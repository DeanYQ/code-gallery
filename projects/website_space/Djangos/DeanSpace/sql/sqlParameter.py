class SqlParameter:
    __dict = None

    def __init__(self):
        self.__dict = {}

    def add(self, key, value):
        self.__dict[key] = value

    def getItems(self):
        return self.__dict

    def clear(self):
        self.__dict.clear()

    def isEmpty(self):
        return len(self.__dict) <= 0
