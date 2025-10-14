using System.Numerics;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.IKSolver;

public class FireflyCache: IFireflyCache<JointAngles, Vector3>
{
    private readonly Firefly<JointAngles, Vector3>?[,,] _cache;
    private readonly int _divisionsPerDimension;
    private readonly float[] _divisions;
    private readonly IRobotModel _robotModel;
    private JointAngles? _defaultMinValues;
    private JointAngles? _defaultMaxValues;
    
    public FireflyCache(IRobotModel robotModel, int divisionsPerDimension)
    {
        _divisionsPerDimension = divisionsPerDimension;
        _cache = new Firefly<JointAngles, Vector3>[divisionsPerDimension, divisionsPerDimension, divisionsPerDimension];
        InitializeCacheValuesToNull();
        _robotModel = robotModel;
        _divisions = CalculateDivisions();
    }
    
    public JointAngleBoundaries GetCachedBoundaries(Vector3 location)
    {
        Firefly<JointAngles, Vector3>? cachedValue = GetCachedValue(location);

        if (cachedValue == null)
        {
            return GetDefaultBoundaries();
        }
        
        JointAngleBoundaries boundaries = MakeBoundariesForCachedValue(cachedValue);
        return boundaries;
    }

    private JointAngleBoundaries MakeBoundariesForCachedValue(Firefly<JointAngles, Vector3> cachedValue)
    {
        List<double> minValues = [];
        List<double> maxValues = [];

        for (int i = 1; i <= _robotModel.GetDoF(); i++)
        {
            double minValue = _robotModel.GetMinAngle(i);
            double maxValue = _robotModel.GetMaxAngle(i);
            double rangeWidth = (maxValue - minValue) / _divisionsPerDimension;
            minValues.Add(cachedValue.Data.GetJoint(i) - rangeWidth);
            maxValues.Add(cachedValue.Data.GetJoint(i) + rangeWidth);
        }

        JointAngles minJointAngles = new(minValues.ToArray());
        JointAngles maxJointAngles = new(maxValues.ToArray());

        return new JointAngleBoundaries(minJointAngles, maxJointAngles);
    }
    
    public void CacheSwarm(Firefly<JointAngles, Vector3>[] fireflies)
    {
        foreach (Firefly<JointAngles, Vector3> firefly in fireflies)
        {
            CacheFirefly(firefly);
        }
    }

    public bool HasCachedValue(Vector3 location)
    {
        Firefly<JointAngles, Vector3>? cachedValue = GetCachedValue(location);
        return cachedValue != null;
    }

    private float[] CalculateDivisions()
    {
        // multiply by two because it can reach that far in both x directions
        float divisionWidth = _robotModel.MaxReach * 2 / _divisionsPerDimension;
        float[] divisions = new float[_divisionsPerDimension + 1];
        for (int i = 0; i < divisions.Length; i++)
        {
            divisions[i] = i * divisionWidth;
        }
        
        return divisions;
    }
    
    private void CacheFirefly(Firefly<JointAngles, Vector3> firefly)
    {
        int x = GetCacheCoordinate(firefly.Output.X);
        int y = GetCacheCoordinate(firefly.Output.Y);
        int z = GetCacheCoordinate(firefly.Output.Z);
        
        if (_cache[x, y, z] != null)
        {
            _cache[x, y, z] = firefly;
        }
    }

    private int GetCacheCoordinate(float value)
    {
        for (int i = 0; i <= _divisionsPerDimension; i++)
        {
            if (value < _divisions[i])
            {
                return i;
            }
        }
        
        return _divisionsPerDimension - 1;
    }

    private void InitializeCacheValuesToNull()
    {
        for (int x = 0; x < _divisionsPerDimension; x++)
        {
            for (int y = 0; y < _divisionsPerDimension; y++)
            {
                for (int z = 0; z < _divisionsPerDimension; z++)
                {
                    _cache[x, y, z] = null;
                }
            }
        }
    }

    private Firefly<JointAngles, Vector3>? GetCachedValue(Vector3 location)
    {
        int cacheXCoordinate = GetCacheCoordinate(location.X);
        int cacheYCoordinate = GetCacheCoordinate(location.Y);
        int cacheZCoordinate = GetCacheCoordinate(location.Z);

        return CacheHasValueInPosition(cacheXCoordinate, cacheYCoordinate, cacheZCoordinate) ? 
            _cache[cacheXCoordinate, cacheYCoordinate, cacheZCoordinate]! : 
            null;
    }

    private bool CacheHasValueInPosition(int x, int y, int z)
    {
        if (x > 0 && x < _divisionsPerDimension && y > 0 && y < _divisionsPerDimension && z > 0 &&
            z < _divisionsPerDimension)
        {
            return _cache[x, y, z] != null;
        }
        
        return false;
    }

    private JointAngleBoundaries GetDefaultBoundaries()
    {
        return new JointAngleBoundaries(GetDefaultMinValues(), GetDefaultMaxValues());
    }

    private JointAngles GetDefaultMinValues()
    {
        if (_defaultMinValues is not null)
        {
            return _defaultMinValues;
        }
        
        List<double> minValues = [];
        
        for(int i = 1; i <= _robotModel.GetDoF(); i++)
        {
            minValues.Add(_robotModel.GetMinAngle(i));
        }

        _defaultMinValues = new JointAngles(minValues.ToArray());
        
        return _defaultMinValues;
    }
    
    private JointAngles GetDefaultMaxValues()
    {
        if (_defaultMaxValues is not null)
        {
            return _defaultMaxValues;
        }
        
        List<double> maxValues = [];
        
        for(int i = 1; i <= _robotModel.GetDoF(); i++)
        {
            maxValues.Add(_robotModel.GetMaxAngle(i));
        }

        _defaultMaxValues = new JointAngles(maxValues.ToArray());
        
        return _defaultMaxValues;
    }
}