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
                #self.wfile.write("Autoupdate erzeugt ein neues Update...")
                
                # Starte Autoupdate
                os.system("python3 " + path_autoupdate)
                return
            return

def main():
    try:
        server = HTTPServer(('', port), MyHandler)
        print("Starte HTTP Server...")
        server.serve_forever()
    except KeyboardInterrupt:
        print("Fahre Server herunter...")
        server.socket.close()

if __name__ == '__main__':
    main()
