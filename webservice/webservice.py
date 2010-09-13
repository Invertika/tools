#!/usr/bin/env python
# -*- coding: utf-8 -*-

import string,cgi,time, os 
from http.server import BaseHTTPRequestHandler,HTTPServer

port = 8080
path_autoupdate = "/home/manaserv/autoupdate.py"
path_gameserverlog = "/home/manaserv/.manaserv-game.log"
path_accountserverlog = "/home/manaserv/.manaserv-account.log"
path_server_restart_skript = "/home/manaserv/restart-server.sh"
path_logfile = "/home/manaserv/autoupdate.log"
password = "geheim"

class MyHandler(BaseHTTPRequestHandler):

    def do_GET(self):
            if self.path.endswith("gslog?password="+password):
                self.send_response(200)
                self.send_header('Content-type', 'text')
                self.end_headers()
                f = open(path_gameserverlog, "r")
                gmslog = f.read()
                f.close()
                self.wfile.write(gmslog.encode())
                return
            elif self.path.endswith("aslog?password="+password):
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
                log = os.system("python3 " + path_autoupdate)
                with open(path_logfile, mode='a', encoding='utf-8') as a_file:
                    a_file.write(log)

                # Rückmeldung
                self.wfile.write(log.encode())
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
            elif self.path.endswith("/"):
                self.send_response(200)
                self.send_header('Content-type', 'text/html')
                self.end_headers()
                f = open("./webservice.html", "r")
                index = f.read()
                f.close()
                self.wfile.write(index.encode())
                return
            else:
                self.send_response(200)
                self.send_header('Content-type', 'text/html')
                self.end_headers()
                f = open("./webservice-error.html", "r")
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
