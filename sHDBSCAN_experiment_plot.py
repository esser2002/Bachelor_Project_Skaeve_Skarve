import math

import matplotlib.pyplot as plt
import pandas as pd
import numpy as np


rawdata = pd.read_csv(r"C:\Users\jonas\OneDrive\Dokumenter\ITU\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\sHDBSCAN_runtime.csv")
print(rawdata)
averageddata = rawdata.groupby('datasize').mean()
print(averageddata)

# Take averages of data
plt.plot(averageddata['sHDBSCANtime'], label="sHDBSCAN", marker='o', markersize=4, linewidth=2)
plt.plot(averageddata['HDBSCANtime'], label="HDBSCAN", marker='o', markersize=4, linewidth=2)


plt.xlabel('input size')
plt.ylabel('runtime (s)')
plt.ylim(bottom=-200, top=4300)

# Create a smooth curve
n_smooth = np.linspace(averageddata.index.min(), averageddata.index.max(), 500)

# Set theoretical plots (achieved by linear interpolation of input data points, in this case between the HDBSCAN and sHDBSCAN for n=4000 and n=5000)
start_datasize = 4000.1703150472863
start_value = 20.2613047045201

constant = start_value / (start_datasize**2)
quadratic = n_smooth**2 * constant
plt.plot(n_smooth, quadratic, label="$n^2$", linestyle='--')

constant = start_value / (start_datasize * np.sqrt(start_datasize))
linearroot = n_smooth * np.sqrt(n_smooth) * constant
plt.plot(n_smooth, linearroot, label="$n\cdot \sqrt{n}$", linestyle='--')

constant = start_value / (start_datasize * np.log2(start_datasize))
linearithmic = n_smooth * np.log2(n_smooth) * constant
plt.plot(n_smooth, linearithmic, label="$n\cdot \log (n)$", linestyle='--')

constant = start_value / start_datasize
linear = n_smooth*constant
plt.plot(n_smooth, linear, label="$n$", linestyle='--')

# draw graph
plt.legend(loc='upper left', fontsize=10)

plt.show()