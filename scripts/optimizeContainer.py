# optimizeContainer.py

# from __future__ import print_function
# import math

def optimizeContainer(L1, W1, H1, L2, W2, H2)
    newLength = L1 + L2
    newWidth = W1 + W2
    newHeight = H1 + H2
    newVolume = newLength * newWidth * newHeight
    return [newLength, newWidth, newHeight, newVolume]