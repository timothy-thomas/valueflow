import json

parameters = dict()

def add(x, *args):
  for arg in args:
    x = x + arg
  return x

def mult(x, *args):
  for arg in args:
    x = x * arg
  return x

def max (x, *args):
  for arg in args:
    if arg > x:
      x = arg
  return x

simpleResults = list()
complexResults = list()
pythonResults = list()

parameters["Component"] = dict()

#----------------- Pass 0 ------------------
parameters["Component"]["Width"] = $Width
parameters["Component"]["Length"] = $Length
parameters["Component"]["Height"] = $Height


#----------------- Pass 1 ------------------

complexResults.append(((((parameters["Component"]["Length"])*(parameters["Component"]["Width"]))*(parameters["Component"]["Height"]))))

#----------------- Pass 2 ------------------
parameters["Component"]["Volume"] = complexResults[0]


#----------------- Pass 3 ------------------

#------ Done! (No new values found.) -------

print json.dumps(parameters, indent=2, sort_keys=True)

with open('output.json', 'w') as f_out:
    json.dump(parameters, f_out, indent=2, sort_keys=True)
