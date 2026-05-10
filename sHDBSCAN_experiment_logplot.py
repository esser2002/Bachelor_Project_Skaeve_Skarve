import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

rawdata = pd.read_csv(r"C:\Users\jonas\OneDrive\Dokumenter\ITU\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\sHDBSCAN_runtime.csv")
print(rawdata)

# Take averages of data
averageddata = rawdata.groupby('datasize').mean()
print(averageddata)

# Make linear regression on a log-log space 
n = averageddata.index      
sHDBSCANtimes = averageddata['sHDBSCANtime']
HDBSCANtimes = averageddata['HDBSCANtime']

log_n = np.log(n)
log_tsHDBSCAN = np.log(sHDBSCANtimes)
log_tHDBSCAN = np.log(HDBSCANtimes)

coeffssHDBSCAN = np.polyfit(log_n, log_tsHDBSCAN, deg=1)
slope = coeffssHDBSCAN[0]        
sHDBSCANintercept = coeffssHDBSCAN[1]

print(f"sHDBSCAN")
print(f"Estimated exponent: k ≈ {slope:.3f}")
print(f"Suggests O(n^{slope:.2f}) complexity")

coeffsHDBSCAN = np.polyfit(log_n, log_tHDBSCAN, deg=1)
slope = coeffsHDBSCAN[0]        
HDBSCANintercept = coeffsHDBSCAN[1] 

print(f"HDBSCAN")
print(f"Estimated exponent: k ≈ {slope:.3f}")
print(f"Suggests O(n^{slope:.2f}) complexity")

# Plot data
fig, ax = plt.subplots()

ax.plot(averageddata['sHDBSCANtime'], label="sHDBSCAN", marker='o', markersize=4, linewidth=2)
ax.plot(averageddata['HDBSCANtime'], label="HDBSCAN", marker='o', markersize=4, linewidth=2)


ax.set_xlabel('input size')
ax.set_ylabel('runtime (s)')

n_smooth = np.linspace(averageddata.index.min(), averageddata.index.max(), 500)

# Set theoretical plots
start_datasize = 4000.1703150472863
start_value = 20.2613047045201
#start_datasize = 500
#start_value = averageddata['sHDBSCANtime'][start_datasize]

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

# add labels and shit
ax.set_xscale('log', base=2)
ax.set_xticks([500, 1000, 2000, 4000, 8000, 16000, 32000, 64000])
ax.set_xticklabels(['500', '1k', '2k', '4k', '8k', '16k', '32k', '64k'])

ax.set_yscale('log', base=2)

ax.set_xlabel('input size')
ax.set_ylabel('runtime (s)')

# draw graph
plt.legend(loc='upper left', fontsize=10)

plt.show()