from DeanSpace.sql.sqlCommand import SqlCommand
from DeanSpace.sql.sqlParameter import SqlParameter
from uuid import uuid1
from DeanSpace.extensions import stringEx


class UserInfo:
    user_id = ''
    nickName = ''
    pwd = ''
    cfmPwd = ''
    email = ''


def register(user, password):
    pass


def addUser(userInfo):
    cmd = None
    try:
        param = SqlParameter()
        param.add('uuid', uuid1())
        param.add('nickName', userInfo.nickName)
        param.add('pwd', userInfo.pwd)
        param.add('email', userInfo.email)
        sql = r'insert into Users values (@uuid, @nickName, @pwd, @email, datetime())'
        cmd = SqlCommand(sql, param)
        cmd.executeNoQuery()
    except:
        return False;
    finally:
        if cmd != None:
            cmd.close()
    return True


def getUserByEmail(email):
    if stringEx.isNullOrWhitespace(email):
        return None

    cmd = None
    userInfo = None
    try:
        param = SqlParameter()
        param.add("email", email)
        sql = r'select Id, NickName, Email from Users where Email = @email'
        cmd = SqlCommand(sql, param)
        cursor = cmd.executeQuery()
        for row in cursor:
            userInfo = UserInfo()
            userInfo.email = email
            userInfo.user_id = row[0]
            userInfo.nickName = row[1]
            return userInfo
    except:
        return None
    finally:
        if cmd != None:
            cmd.close()


def getUserById(id):
    if stringEx.isNullOrWhitespace(id):
        return None

    cmd = None
    userInfo = None
    try:
        param = SqlParameter()
        param.add("id", id)
        sql = r'select Id, NickName, Email from Users where Id = @id'
        cmd = SqlCommand(sql, param)
        cursor = cmd.executeQuery()
        for row in cursor:
            userInfo = UserInfo()
            userInfo.user_id = id
            userInfo.nickName = row[1]
            userInfo.email = row[2]
            return userInfo
    except:
        return None
    finally:
        if cmd != None:
            cmd.close()


def validateRegister(userInfo):
    if not userInfo:
        return "Invalid user information."
    else:
        msg = validateEmail(userInfo.email)
        if not stringEx.isNullOrWhitespace(msg):
            return msg
        msg = validateNickName(userInfo.nickName)
        if not stringEx.isNullOrWhitespace(msg):
            return msg
        msg = validatePassword(userInfo.pwd, userInfo.cfmPwd)
        if not stringEx.isNullOrWhitespace(msg):
            return msg
        return None


def validateEmail(email):
    if stringEx.isNullOrWhitespace(email):
        return r"email can't be empty."

    try:
        sql = r'select count(*) from Users where email = @email'
        param = SqlParameter()
        param.add('email', email)
        cmd = SqlCommand(sql, param)
        scalar = cmd.executeScalar()
    except:
        return "Invalid email data."
    finally:
        cmd.close()
    if scalar > 0:
        return r'The email has already existed.'
    else:
        return ''

def validateNickName(nickName):
    if stringEx.isNullOrWhitespace(nickName):
        return r"Nick name can't be empty."
    try:
        sql = r'select count(*) from Users where NickName = @nickName'
        param = SqlParameter()
        param.add('nickName', nickName)
        with SqlCommand(sql, param) as cmd:
            scalar = cmd.executeScalar()
            if scalar > 0:
                return r'The nick name has already existed.'
    except:
        return r'Invalid nick name.';
    return '' 

def validatePassword( pwd, cfmPwd):
    #TODO: use regex to validate the pwd chars.
    if pwd == '':
        return r"The password you entered can't be empty."
    if pwd != cfmPwd:
        return r'The passwords you entered are not consistent.'
    return ''

def validateLogin(email, pwd):
    error = r'Invalid email or password.'
    if stringEx.isNullOrWhitespace(email) or stringEx.isNullOrWhitespace(pwd):
        return error;

    try:
        sql = r'select count(*) from Users where Email = @email and Pwd =@pwd'
        param = SqlParameter()
        param.add('email', email)
        param.add('pwd', pwd)
        cmd = SqlCommand(sql, param)
        scalar = cmd.executeScalar()
    except:
        return "Invalid email data."
    finally:
        cmd.close()
    if scalar > 0:
        return ''
    else:
        return error

