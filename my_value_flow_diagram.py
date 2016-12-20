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
  
  parameters = dict()
  parameters["FullExample"] = dict()
  parameters["FullExample"]["PythonContainer"] = dict()
  result = list()
  
  # Build up constants
  parameters["FullExample"]["height1"] = 4
  parameters["FullExample"]["height2"] = 6
  parameters["FullExample"]["width1"] = 2
  parameters["FullExample"]["width2"] = 7
  parameters["FullExample"]["length1"] = 6
  parameters["FullExample"]["length2"] = 4
  
  # TODO Accept keyword arguments as input to overwrite constants
  
  # Build up functions
  parameters["FullExample"]["SimpleContainer"] = dict()
  parameters["FullExample"]["SimpleContainer"]["Length"] = max(parameters["FullExample"]["length1"], parameters["FullExample"]["length2"])
  parameters["FullExample"]["SimpleContainer"]["Width"] = max(parameters["FullExample"]["width1"], parameters["FullExample"]["width2"])
  parameters["FullExample"]["SimpleContainer"]["Height"] = add(parameters["FullExample"]["height1"], parameters["FullExample"]["height2"])
  parameters["FullExample"]["SimpleContainer"]["Volume"] = mult(max(parameters["FullExample"]["length1"], parameters["FullExample"]["length2"]),max(parameters["FullExample"]["width1"], parameters["FullExample"]["width2"]),add(parameters["FullExample"]["height1"], parameters["FullExample"]["height2"]))
  
  # Test Python Feature
  import optimizeContainer
  result.append(optimizeContainer.optimizeContainer(parameters["FullExample"]["height1"],
                                parameters["FullExample"]["height2"],
                                parameters["FullExample"]["width1"],
                                parameters["FullExample"]["width2"],
                                parameters["FullExample"]["length1"],
                                parameters["FullExample"]["length2"]))
  parameters["FullExample"]["PythonContainer"]["Length"] = result[0][0]
  parameters["FullExample"]["PythonContainer"]["Width"] = result[0][1]
  parameters["FullExample"]["PythonContainer"]["Height"] = result[0][2]
  parameters["FullExample"]["PythonContainer"]["Volume"] = result[0][3]
  
  print json.dumps(parameters, indent=2, sort_keys=True)
  