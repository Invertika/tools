#!/usr/bin/env python
# -*- coding: utf-8 -*-

import string,cgi,time, os 
from http.server import BaseHTTPRequestHandler,HTTPServer

port = 8080
path_autoupdate = "/home/manaserv/autoupdate.py"
password = "geheim"

class MyHandler(BaseHTTPRequestHandler):

    def do_GET(self):
            if self.path.endswith("restart?password="+password):
                self.send_response(200)
                self.send_header('Content-type', 'text/html')
                self.end_headers()

                # Starte Autoupdate
                os.system("python3 " + path_autoupdate)

                # Rückmeldung
                self.wfile.write("Autoupdate wurde erzeugt und Server neugestartet.".encode())
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
