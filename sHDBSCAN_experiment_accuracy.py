import math

import matplotlib.pyplot as plt
import pandas as pd
import numpy as np


rawdata = pd.read_csv(r"C:\Users\jonas\OneDrive\Dokumenter\ITU\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\results\sHDBSCAN_runtime.csv")
print(rawdata)
averageddata = rawdata.groupby('datasize').mean()
print(averageddata)

fig, ax = plt.subplots(figsize=(7, 3))

# Take averages of data
ax.plot(averageddata['pcc'], label="pcc", marker='o', markersize=4, linewidth=2)
ax.plot(averageddata['mr'], label="mr", marker='o', markersize=4, linewidth=2)


ax.set_xlabel('input size')
ax.set_ylabel('similarity')
ax.set_ylim(bottom=0.95, top=1.001)

# Create a smooth curve
n_smooth = np.linspace(averageddata.index.min(), averageddata.index.max(), 500)

# draw graph
plt.legend(loc='lower left', fontsize=10)

plt.show()