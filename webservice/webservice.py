#!/usr/bin/env python
# -*- encoding: utf-8 -*-

import sqlite3, hashlib, string, os
from bottle import get, post, route, run, debug, template, request, response, HTTPResponse, redirect

debug_mode = True
port = 8080
path_gameserverlog = "/home/manaserv/.manaserv-game.log"
path_accountserverlog = "/home/manaserv/.manaserv-account.log"

def get_aslog():
    f = open(path_accountserverlog, "r")
    aslog = f.read()
    f.close()
    return aslog.replace("\n", "<br>")

def get_gslog():
    f = open(path_gameserverlog, "r")
    gslog = f.read()
    f.close()
    return gslog.replace("\n", "<br>")

def change_pw(username, password):
    con = sqlite3.connect('webservice.db')
    con.execute("UPDATE users SET password=? WHERE username=?", (hashlib.md5(password.encode()).hexdigest(),username))
    con.commit()
    return "Das Passwort wurde geändert!"

def is_valid_user(username, password):
    con = sqlite3.connect('webservice.db')
    result = con.execute("SELECT username,password FROM users WHERE username=? LIMIT 1", (username,))
    con.commit()
    data = result.fetchone()
    if(data):
        # Benutzer in der Datenbank
        return (data[0]==username) and (data[1]==hashlib.md5(password.encode()).hexdigest())
    else:
        # Benutzer nicht in der Datenbank
        return False

@route('/')
def static_login():
    output = template('login')
    return str(output)

@route('/login', method='POST')
def do_login():
    username = request.POST.get('username', '')
    password = request.POST.get('password', '')
    if is_valid_user(username, password):
        # Login ist valide - Hauptseite ausgeben
        response.set_cookie('username', username)
        response.set_cookie('password', password)
        output = template('choice')
    else:
        # Login ist invalide - neuer Versuch
        output = "Leider sind diese Zugangsdaten ungültig.\n<hr>\n"
        output += template('login')
    return output

@route('/choice')
def give_choice():
    username = request.COOKIES.get('username', '')
    password = request.COOKIES.get('password', '')
    if is_valid_user(username, password):
        output = template('choice')
    else:
        # Login ist invalide - neuer Versuch
        output = "Leider sind diese Zugangsdaten ungültig.\n<hr>\n"
        output += template('login')
    return output

@route('/choice/:choice')
def made_choice(choice):
    username = request.COOKIES.get('username', '')
    password = request.COOKIES.get('password', '')
    if is_valid_user(username, password):
        if (choice == "aslog"):
            output = template('choice')
            output += "<hr>"
            output += get_aslog()
        elif (choice == "gslog"):
            output = template('choice')
            output += "<hr>"
            output += get_gslog()
        else:
            output = "Ung&uuml;ltige Auswahl!\n<hr>\n"
            output += give_choice()
    else:
        # Login ist invalide - neuer Versuch
        output = "Leider sind diese Zugangsdaten ungültig.\n<hr>\n"
        output += template('login')
    return output

@route('/choice/changepw', method='GET')
def changepw():
    username = request.COOKIES.get('username', '')
    password = request.COOKIES.get('password', '')
    if is_valid_user(username, password):
        output = template('choice')
        output += "<hr>"
        output += template('changepw')
    else:
        # Login ist invalide - neuer Versuch
        output = "Leider sind diese Zugangsdaten ungültig.\n<hr>\n"
        output += template('login')
    return output

@route('/choice/changepw', method='POST')
def changepw():
    username = request.COOKIES.get('username', '')
    password = request.COOKIES.get('password', '')
    if is_valid_user(username, password):
        passwd_old = request.POST.get('passwordold', '')
        passwd_new = request.POST.get('passwordnew', '')
        passwd_new2 = request.POST.get('passwordnew2', '')
        if (passwd_old == password):
            if (passwd_new == passwd_new2):
                if (len(passwd_new) > 5):
                    output = template('choice')
                    output += "<hr>"
                    output += change_pw(username, passwd_new)
                else:
                    output = template('choice')
                    output += "<hr>"
                    output += "Das neue Passwort muss mindestens 6 Zeichen haben!"
                    output += "<hr>"
                    output += template('changepw')
            else:
                output = template('choice')
                output += "<hr>"
                output += "Die beiden neuen Passwörter stimmen nicht überein!"
                output += "<hr>"
                output += template('changepw')
        else:
            output = template('choice')
            output += "<hr>"
            output += "Das eingegebene Passwort ist leider falsch!"
            output += "<hr>"
            output += template('changepw')
    else:
        # Login ist invalide - neuer Versuch
        output = "Leider sind diese Zugangsdaten ungültig.\n<hr>\n"
        output += template('login')
    return output
@route('/logout')
def logout():
    response.set_cookie('username','')
    response.set_cookie('password','')
    output = "Sie wurden ausgeloggt!\n<hr>\n"
    output += template('login')
    return output

@route('/:page#.*#')
def allother(page):
    redirect("http://invertika.org/")

debug(debug_mode)
run(reloader=True, host="0.0.0.0", port=port)
