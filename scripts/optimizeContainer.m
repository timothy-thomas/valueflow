function [Length, Width, Height] = OptimizeContainer(L1, W1, H1, L2, W2, H2)
%% OptimizeContainer Model
%
% This function optimizes the container size necessary to carry
% the two objects specified by their respective lengths (LX),
% widths (WX), and heights (HX)

Length = L1 + L2
Width  = W1 + W2
Height = H1 + H2

end