import os
import sys
import json
from string import Template

def read_testbench_manifest_parameters(testbench_manifest):
    parameters_dict = dict()
    print "Reading parameters from testbench_manifest.json..."
    print
    for parameter in testbench_manifest['Parameters']:
        parameters_dict[parameter['Name']] = parameter['Value']
        print parameter['Name'] + ": " + str(parameter['Value'])
    print
    return parameters_dict

def build_script_failed(testbench_manifest, err_message):
    #Mark as "FAILED" and clean up testbench_manifest
    testbench_manifest["Status"] = "FAILED"
    testbench_manifest["Artifacts"] = []
    testbench_manifest["Metrics"] = []

    #Pass error message to Manifest
    testbench_manifest["ErrorMessage"] = err_message

    #Save the testbench_manifest
    with open('testbench_manifest.json', 'w') as f_out:
        json.dump(testbench_manifest, f_out, indent=2)

if __name__ == '__main__':
    print "Running " + str(__file__) + "..."

    #Populate the 'output.py' script

    #Obtain testbench configuration
    with open('testbench_manifest.json', 'r') as f_in:
        testbench_manifest = json.load(f_in)

    replacement_dict = read_testbench_manifest_parameters(testbench_manifest)

    with open('output_template.py', 'r') as f_in:
        output_template = Template(f_in.read())

    #Substitute parameters into 'output.py' Script
    print "Substituting parameters into template script..."
    try:
        output = output_template.substitute(replacement_dict)
    except KeyError, Argument:
        build_script_failed(testbench_manifest, "Error: Run Aborted: Needed value {} does not exist in testbench_manifest.json.".format(Argument))
        print "Error: {} does not exist in testbench_config.".format(Argument)
        print "Execution Aborted."
        sys.exit(1)

    print "Saving script to 'output.py'..."
    with open('output_parameterized.py', 'w') as f_out:
        f_out.write(output)

    #Record the new artifact
    print "Recording artifacts..."
    if os.path.exists('script.xfoil'):
        testbench_manifest["Artifacts"].append({"Tag": "ValueFlowNetwork", "Location": "output.py"})

    #Save the testbench_manifest
    with open('testbench_manifest.json', 'w') as f_out:
        json.dump(testbench_manifest, f_out, indent=2)

    print "Done."
