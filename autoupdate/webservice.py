#!/usr/bin/env python
# -*- coding: utf-8 -*-

import string,cgi,time, os 
from http.server import BaseHTTPRequestHandler,HTTPServer

port = 8080
path_autoupdate = "/home/manaserv/autoupdate.py"
path_gameserverlog = ""
path_accountserverlog = ""
path_server_restart_skript = ""
password = "geheim"

class MyHandler(BaseHTTPRequestHandler):

    def do_GET(self):
            if self.path.endswith("gmslog?password="+password):
                self.send_response(200)
                self.send_header('Content-type', 'text')
                self.end_headers()
                f = open(path_gameserverlog, "r")
                gmslog = f.read()
                f.close()
                self.wfile.write(gmslog.encode())
                return
            elif self.path.endswith("acclog?password="+password):
                self.send_response(200)
                self.send_header('Content-type', 'text')
                self.end_headers()
                f = open(path_accountserverlog, "r")
                acclog = f.read()
                f.close()
                self.wfile.write(acclog.encode())
                return
            elif self.path.endswith("autoupdate?password="+password):
                self.send_response(200)
                self.send_header('Content-type', 'text/html')
                self.end_headers()

                # Starte Autoupdate
                os.system("python3 " + path_autoupdate)

                # Rückmeldung
                self.wfile.write("Autoupdate wurde erzeugt und Server neugestartet.".encode())
                return
            elif self.path.endswith("restart?password="+password):
                self.send_response(200)
                self.send_header('Content-type', 'text/html')
                self.end_headers()

                # Starte Server neu
                os.system(path_server_restart_skript)

                # Rückmeldung
                self.wfile.write("Server wurde neugestartet.".encode())
                return
            else:
                self.send_response(200)
                self.send_header('Content-type', 'text/html')
                self.end_headers()
                f = open("./index.html", "r")
                index = f.read()
                f.close()
                self.wfile.write(index.encode())
                return
            return

def main():
    try:
        server = HTTPServer(('', port), MyHandler)
        print("HTTP Server laeuft...")
        server.serve_forever()
    except KeyboardInterrupt:
        print("Beende HTTP Server...")
        server.socket.close()

if __name__ == '__main__':
    main()
