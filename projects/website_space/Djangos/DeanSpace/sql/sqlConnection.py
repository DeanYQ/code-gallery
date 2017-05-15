import sqlite3

# database name
database = 'db.sqlite3'

class SqlConnection():
    __conn = None

    def __init__(self):
        self.__conn = SqlConnection.connect(database)

    @staticmethod
    def connect(db):
        return sqlite3.connect(db)

    def close(self):
        if self.__conn:
            self.__conn.close()

    def executeQuery(self, sql):
        if self.__conn:
            # return cursor
            return self.__conn.execute(sql)

    def executeNoQuery(self, sql):
        if self.__conn:
            self.__conn.execute(sql)
            self.__conn.commit()
            self.close()