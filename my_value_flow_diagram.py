import json

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
  
if __name__ == '__main__': 
  
  # Initialize all dictionaries
  parameters = dict()
  parameters["FullExample"] = dict()
  parameters["FullExample"]["SimpleContainer"] = dict()
  parameters["FullExample"]["ComplexContainer"] = dict()
  parameters["FullExample"]["PythonContainer"] = dict()
  
  # Initialize list for python block results
  pythonResults = list()
  
  # Build up constants
  parameters["FullExample"]["Box1Height"] = 4
  parameters["FullExample"]["Box2Height"] = 6
  parameters["FullExample"]["Box1Width"] = 2
  parameters["FullExample"]["Box2Width"] = 7
  parameters["FullExample"]["Box1Length"] = 6
  parameters["FullExample"]["Box2Length"] = 4
  
  # TODO Accept keyword arguments as input to overwrite constants
  
  # Test Simple Feature
  parameters["FullExample"]["SimpleContainer"]["Length"] = max(parameters["FullExample"]["Box1Length"], parameters["FullExample"]["Box2Length"])
  parameters["FullExample"]["SimpleContainer"]["Width"] = max(parameters["FullExample"]["Box1Width"], parameters["FullExample"]["Box2Width"])
  parameters["FullExample"]["SimpleContainer"]["Height"] = add(parameters["FullExample"]["Box1Height"], parameters["FullExample"]["Box2Height"])
  parameters["FullExample"]["SimpleContainer"]["Volume"] = mult(max(parameters["FullExample"]["Box1Length"], parameters["FullExample"]["Box2Length"]),max(parameters["FullExample"]["Box1Width"], parameters["FullExample"]["Box2Width"]),add(parameters["FullExample"]["Box1Height"], parameters["FullExample"]["Box2Height"]))
  
  # Test Simple Feature
  parameters["FullExample"]["ComplexContainer"]["Length"] = max(parameters["FullExample"]["Box1Length"], parameters["FullExample"]["Box2Length"])
  parameters["FullExample"]["ComplexContainer"]["Width"] = max(parameters["FullExample"]["Box2Width"], parameters["FullExample"]["Box1Width"])
  parameters["FullExample"]["ComplexContainer"]["Height"] = add(parameters["FullExample"]["Box1Height"], parameters["FullExample"]["Box2Height"])
  parameters["FullExample"]["ComplexContainer"]["Volume"] = mult(max(parameters["FullExample"]["Box1Width"], parameters["FullExample"]["Box2Width"]),max(parameters["FullExample"]["Box1Length"], parameters["FullExample"]["Box2Length"]),add(parameters["FullExample"]["Box1Height"], parameters["FullExample"]["Box2Height"]))
  
  # Test Python Feature
  import optimizeContainer
  pythonResults.append(optimizeContainer.optimizeContainer(
    parameters["FullExample"]["Box1Height"],
    parameters["FullExample"]["Box2Height"],
    parameters["FullExample"]["Box1Width"],
    parameters["FullExample"]["Box2Width"],
    parameters["FullExample"]["Box1Length"],
    parameters["FullExample"]["Box2Length"]))
  parameters["FullExample"]["PythonContainer"]["Length"] = pythonResults[0][0]
  parameters["FullExample"]["PythonContainer"]["Width"] = pythonResults[0][1]
  parameters["FullExample"]["PythonContainer"]["Height"] = pythonResults[0][2]
  parameters["FullExample"]["PythonContainer"]["Volume"] = pythonResults[0][3]
  
  print json.dumps(parameters, indent=2, sort_keys=True)
  