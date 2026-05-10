set D=10
set k=10
set m=10
set epsilon=0.11
set minPts=50
set inputdata=C:\Users\frede\Documents\GitHub\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\mnist_full.csv
set clusteroutput=C:\Users\frede\Documents\GitHub\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\mnist_full_sDBSCAN_Clusters.csv
set statsoutput=C:\Users\frede\Documents\GitHub\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\mnist_full_statsoutput.csv
set normalisedDataRaw=C:\Users\frede\Documents\GitHub\Bachelor_Project_Skaeve_Skarve\Code\BA_Scalable_Clustering\data\out\normalised_mnist_full.csv


cd ..\sDBSCAN\
dotnet run "%inputdata%" "%clusteroutput%" "%statsoutput%" "%D%" "%k%" "%m%" "%epsilon%" "%minPts%"
cd .\python\
echo %normalisedDataRaw% %clusteroutput% %epsilon% %minPts% | uv run nmi.py
cd ..
