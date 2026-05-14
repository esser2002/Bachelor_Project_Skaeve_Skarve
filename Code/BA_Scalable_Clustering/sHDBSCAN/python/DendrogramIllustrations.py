import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import sklearn.datasets as data
import hdbscan

# Plot styling
sns.set_context('poster')
sns.set_style('white')

plot_kwds = {
    'alpha': 0.5,
    's': 80,
    'linewidths': 0
}


test_data = np.array([
    # Cluster 1
    [0.26,0.94],  # A
    [0.382,3.79],  # B
    [2.8,  1.92],  # C
    [3.29, 1],  # D
    [1.5, 0],  # E
    [2.58, 2.87],  # F
    [1.66, 4.43],  # G
    [1.26, 1.99],  # H
    [0.2, 2.24],  # I
    [1.64, 0.98],  # J

    # Cluster 2
    [5.52, 5.88],  # K
    [5.22, 3.72],  # L
    [4.88, 4.46],  # M
    [6.56, 4.46],  # N
    [5.80, 4.66],  # O
    [4.50, 5.18],  # P
    [6.48, 5.18],  # Q
    [5.54, 5.18],  # R
    [5.62, 4.06],  # S
    [6.30, 4.06],  # T

    # Cluster 3
    [7.22, 2.94],  # U
    [6.56, 2.94],  # V
    [7.26, 2.60],  # W
    [6.78, 2.68],  # Z
    [6.40, 2.56],  # A1
    [6.98, 2.28],  # B1
    [7.46, 2.20],  # C1
])

# Show raw data
plt.scatter(test_data[:, 0], test_data[:, 1], color='b', **plot_kwds)
plt.show()

# Run HDBSCAN
clusterer = hdbscan.HDBSCAN(
    min_cluster_size=5,
    gen_min_span_tree=True
)

clusterer.fit(test_data)

# Print model summary
print(clusterer)

# Plot clustering result
palette = sns.color_palette()

cluster_colors = [
    sns.desaturate(palette[col], sat)
    if col >= 0 else (0.5, 0.5, 0.5)
    for col, sat in zip(clusterer.labels_, clusterer.probabilities_)
]
clusterer.single_linkage_tree_.plot(cmap='viridis', colorbar=True)
plt.show()
clusterer.condensed_tree_.plot()
plt.show()
plt.scatter(
    test_data[:, 0],
    test_data[:, 1],
    c=cluster_colors,
    **plot_kwds
)

plt.show()