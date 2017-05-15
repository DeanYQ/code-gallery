from DeanSpace.dataModels import session, users
from DeanSpace.extensions import stringEx


class Auth():
    session_ = None
    user_ = None

    def setAuthByEmail(self, email: str):
        if stringEx.isNullOrWhitespace(email):
            return

        user = users.getUserByEmail(email)
        if user is not None:
            self.user_ = user
            se = session.getSessionByUser(user.user_id)
            if se is not None:
                self.session_ = se

    def setAuthBySessionId(self, session_id: str):
        if stringEx.isNullOrWhitespace(session_id):
            return

        se = session.getSessionById(session_id)
        if se is not None:
            self.session_ = se
            user = users.getUserById(se.user_id)
            if user is not None:
                self.user_ = user

    def isAuthorized(self):
        if self.session_ is None or self.user_ is None:
            return False
        return True


def getAuth(request):
    if request is not None and request.method == 'GET':
        session_id = request.COOKIES.get('session')
        if session_id is not None and (not stringEx.isNullOrWhitespace(session_id)):
            auth = Auth()
            auth.setAuthBySessionId(session_id)
            return auth
    return None
