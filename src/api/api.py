import json
from flask import Flask, request

json_file = open('api.json')
json_content = ''

for line in json_file:
    json_content += line

dat = json.loads(json_content)

addr = dat['addr']
port = dat['port']
url = dat['posturl']
status = dat['statusurl']

port = int(port)
app = Flask(__name__)


state = False

@app.route(f'/{url}', methods=['POST'])
def toggle():
    global state

    print(request.form['cmd'])
    data = request.form['cmd']

    if data == 'toggle-status':
        state = not state
    
    return f'<h1>{data}</h1>'

@app.route(f'/{status}', methods=['GET'])
def status():
    global state

    dat = '{ \"status\": '+str(state).lower()+' }'
    return dat

app.run(debug=True, host=addr, port=port)
