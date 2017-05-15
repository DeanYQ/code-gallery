from django.shortcuts import render, redirect
from django.http import HttpRequest, HttpResponse
from DeanSpace.dataModels.users import UserInfo
from DeanSpace.dataModels.authorization import Auth
from DeanSpace.dataModels import users, session, authorization
from DeanSpace.extensions import stringEx


def home(request):
    assert isinstance(request, HttpRequest)
    auth = authorization.getAuth(request)
    return render(request, 'DeanSpace/home.html', getAuthDict(auth))


def login(request):
    if request.method == 'POST':
        email = request.POST['email']
        pwd = request.POST['pwd']
        error = users.validateLogin(email, pwd)
        if stringEx.isNullOrWhitespace(error):
            user = users.getUserByEmail(email)
            se = session.Session(user.user_id)
            session.addSession(se)
            auth = Auth()
            auth.setAuthByEmail(email)
            return render(request, 'DeanSpace/home.html', {'auth': auth})
        else:
            return render(request, 'DeanSpace/login.html', {'hasError': True, 'errorMessage': error})
    elif request.method == 'GET':
        return render(request, 'DeanSpace/login.html')

def register(request):
    if request.method == 'GET':
        validation = request.GET.get('validation')
        if validation != None:
            type = request.GET['type']
            value = request.GET['value']
            if type == 'id_email':
                return HttpResponse(users.validateEmail(value))
            elif type == 'id_nickname':
                return HttpResponse(users.validateNickName(value))
            elif type == 'id_enterPwd':
                return HttpResponse(users.validatePassword(value))
        return render(request, 'DeanSpace/signup.html')
    elif request.method == 'POST':
        #return render(request, 'DeanSpace/signup.html', { 'hasError': True})
        info = UserInfo()
        info.email = request.POST['email']
        info.nickName = request.POST['nickName']
        info.pwd = request.POST['pwd']
        info.cfmPwd = request.POST['confirmPwd']
        msg = users.validateRegister(info)
        if not stringEx.isNullOrEmpty(msg):
            return render(request, 'DeanSpace/signup.html', {'hasError': True, 'errorMessage': msg})
        re = users.addUser(info)
        if re:
            user = users.getUserByEmail(info.email)
            se = session.Session(user.user_id)
            session.addSession(se)
            auth = Auth()
            auth.setAuthByEmail(info.email)
            return render(request, 'DeanSpace/signupSuccess.html', {'auth': auth})
            #return render(request, 'DeanSpace/signupSuccess.html', {'session': se, 'user': user})

def logout(request):
    if request.method == 'POST':
        session_id = request.COOKIES.get('session')
        if session_id is not None and (not stringEx.isNullOrWhitespace(session_id)):
            session.removeSessionBySessionid(session_id)
    return render(request, 'DeanSpace/home.html')

def game(request):
    if request.method == 'GET':
        auth = authorization.getAuth(request)
        return render(request, 'DeanSpace/game.html', {'auth': auth})

def card(request):
        if request.method == 'GET':
            auth = authorization.getAuth(request)
            return render(request, 'DeanSpace/card.html', {'auth': auth})

def success(request):
    return render(request, 'DeanSpace/signupSuccess.html')

def getAuthDict(auth):
    return {'auth': auth}
