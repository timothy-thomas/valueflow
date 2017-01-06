import json

if __name__ == '__main__':
    print "Running " + str(__file__) + "..."

    #Populate the testbench_manifest with the results ------------------------------------------------------------------
    #Open 'output.json'
    print "Opening 'output.json'..."
    with open('output.json', 'r') as f_in:
        output = json.load(f_in)
        
    #Open 'testbench_manifest.json'
    with open('testbench_manifest.json', 'r') as f_in:
        testbench_manifest = json.load(f_in)

    #Save metrics
    print "Saving Metrics to testbench_manifest.json..."
    for metric in testbench_manifest["Metrics"]:
        #Here use metric["Name"] to fine right metric in output
        metric["Value"] = output["Component"]["Volume"]

    testbench_manifest["Status"]="OK"

    #Save the modified testbench_manifest
    with open('testbench_manifest.json', 'w') as f_out:
        json.dump(testbench_manifest, f_out, indent=2)

    print "Done."
