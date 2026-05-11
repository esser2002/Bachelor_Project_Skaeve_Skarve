import matplotlib.pyplot as plt
import pandas as pd
import numpy as np

rawdata = pd.read_csv(r"C:\Users\jonas\OneDrive\Dokumenter\ITU\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\results\sHDBSCAN_runtime.csv")
print(rawdata)

# Take averages of data
averageddata = rawdata.groupby('datasize').mean()
print(averageddata)

# Make linear regression on a log-log space 
n = averageddata.index      
sHDBSCANtimes = averageddata['sHDBSCANtime']
HDBSCANtimes = averageddata['HDBSCANtime']

def linearreg(data):
    log_n = np.log2(n)
    log_t = np.log2(data)
    coeffs = np.polyfit(log_n, log_t, deg=1)
    slope=coeffs[0]
    print(f"Estimated exponent: k ≈ {slope:.3f}")
    print(f"Suggests O(n^{slope:.2f}) complexity")
    intercept = coeffs[1]

    residuals = log_t - np.polyval(coeffs, log_n)
    ss_res = np.sum(residuals**2)
    ss_tot = np.sum((log_t - np.mean(log_t))**2)
    r_squared = 1 - ss_res / ss_tot
    print(f"R² = {r_squared:.4f}")
    return intercept

print(f"sHDBSCAN")
sHDBSCANintercept = linearreg(sHDBSCANtimes) 

print(f"HDBSCAN")
sHDBSCANintercept = linearreg(HDBSCANtimes) 

print(f"n log n")
sHDBSCANintercept = linearreg(np.multiply(n, np.log2(n)))


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