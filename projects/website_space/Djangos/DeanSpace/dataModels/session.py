from uuid import uuid1, uuid3, UUID
from datetime import *
from DeanSpace.sql.sqlCommand import SqlCommand
from DeanSpace.sql.sqlParameter import SqlParameter

class Session():

    session_id = ''
    user_id = ''
    last_logintime = ''

    def __init__(self, user_id = None):
        self.user_id = user_id
        self.session_id = uuid1()
        self.last_logintime = datetime.now()


def addSession(session):
    cmd = None
    try:
        param = SqlParameter()
        param.add('session_id', session.session_id)
        param.add('userId', session.user_id)
        sql = r'insert into UserSessions values (@session_id, @userId, datetime(), NULL)'
        cmd = SqlCommand(sql, param)
        cmd.executeNoQuery()
    except:
        return False;
    finally:
        if cmd != None:
            cmd.close()
    return True


def removeSessionByUser(user_id):
    cmd = None
    try:
        param = SqlParameter()
        param.add('userId', user_id)
        sql = r'delete from UserSessions where userId = @userId'
        cmd = SqlCommand(sql, param)
        cmd.executeNoQuery()
    except:
        return False;
    finally:
        if cmd is not None:
            cmd.close()
    return True


def removeSessionBySessionid(session_id):
    cmd = None
    try:
        param = SqlParameter()
        param.add('sessionId', session_id)
        sql = r'delete from UserSessions where sessionId = @sessionId'
        cmd = SqlCommand(sql, param)
        cmd.executeNoQuery()
    except:
        return False;
    finally:
        if cmd is not None:
            cmd.close()
    return True


def getSessionById(session_id):
    cmd = None
    se = None
    try:
        param = SqlParameter()
        param.add("sessionId", session_id)
        sql = r'select sessionId, userId, lastLoginTime from UserSessions where sessionId = @sessionId'
        cmd = SqlCommand(sql, param)
        cursor = cmd.executeQuery()
        for row in cursor:
            se = Session()
            se.session_id = row[0];
            se.user_id = row[1]
            se.last_logintime = row[2]
            return se
    except:
        return None
    finally:
        if cmd != None:
            cmd.close()


def getSessionByUser(userId):
    cmd = None
    se = None
    try:
        param = SqlParameter()
        param.add("userid", userId)
        sql = r'select sessionId, userId, lastLoginTime from UserSessions where userId = @userid'
        cmd = SqlCommand(sql, param)
        cursor = cmd.executeQuery()
        for row in cursor:
            se = Session()
            se.session_id = row[0];
            se.user_id = row[1]
            se.last_logintime = row[2]
            return se
    except:
        return None
    finally:
        if cmd != None:
            cmd.close()


def checkSessionOvertime(session_id):
    session = getSessionById(session_id)
    if session!= None:
        lastLogin = session.last_logintime
        time = datetime.strptime(lastLogin, '%Y-%m-%d %H:%M:%S.%f');
        delta = datetime.now() - time
        if delta.days < 1:
            return False
    return True

def isAutherized(session_id):
    if session_id == None:
        return False
    else:
        return checkSessionOvertime(session_id)