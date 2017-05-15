from DeanSpace.sql.sqlConnection import SqlConnection

class SqlCommand:
    __conn = None
    __sql = None
    __param = None

    def __init__(self, rawsql, sqlParam = None):
        self.__conn = SqlConnection()
        self.__sql = rawsql
        self.__param = sqlParam
 
    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.close()

    def __checkParam(self):
        return self.__sql and len(self.__sql) > 0

    def __replaceParams(self):
        sql = self.__sql
        if self.__checkParam():
            if self.__param:
                items = self.__param.getItems()
                for key in items:
                    #TODO: use regex to replace with @variable
                    sql = sql.replace('@' + key, '\'' + items[key].__str__() + '\'')
            return sql
        raise Exception('not correct sql command parameters...')

    def executeQuery(self):
        sql = self.__replaceParams()
        return self.__conn.executeQuery(sql)

    def executeScalar(self):
        sql = self.__replaceParams()
        cursor = self.__conn.executeQuery(sql)
        if cursor:
            for row in cursor:
                return row[0]
        return None

    def executeNoQuery(self):
        sql = self.__replaceParams()
        self.__conn.executeNoQuery(sql)

    def close(self):
        if self.__conn:
            self.__conn.close()


