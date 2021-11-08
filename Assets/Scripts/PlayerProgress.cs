using UnityEngine;
using UnityEngine.Networking;

public class PlayerProgress : MonoBehaviour
{

    int _level = 1, _statPoints, _skillPoints;
    float _exp, _nextLevelExp = 100;

    StatsManager _manager;
    public StatsManager manager
    {
        set
        {
            _manager = value;
            _manager.exp = _exp;
            _manager.nextLevelExp = _nextLevelExp;
            _manager.level = _level;
            _manager.statPoints = _statPoints;
            _manager.skillPoints = _skillPoints;
        }
    }

    UserData data;

    public void Load(UserData data)
    {
        this.data = data;
        if (data.level > 0) _level = data.level;
        _statPoints = data.statPoints;
        _skillPoints = data.skillPoints;
        _exp = data.exp;
        if (data.nextLevelExp > 0) _nextLevelExp = data.nextLevelExp;
    }

    public bool RemoveStatPoint()
    {
        if (_statPoints > 0)
        {
            data.statPoints = --_statPoints;
            if (_manager != null) _manager.statPoints = _statPoints;
            return true;
        }
        return false;
    }

    public bool RemoveSkillPoint()
    {
        if (_skillPoints > 0)
        {
            data.skillPoints = --_skillPoints;
            if (_manager != null) _manager.skillPoints = _skillPoints;
            return true;
        }
        return false;
    }

    public void AddExp(float addExp)
    {
        data.exp = _exp += addExp;
        while (_exp >= _nextLevelExp)
        {
            data.exp = _exp -= _nextLevelExp;
            LevelUP();
        }
        if (_manager != null)
        {
            _manager.exp = _exp;
            _manager.level = _level;
            _manager.nextLevelExp = _nextLevelExp;
            _manager.statPoints = _statPoints;
            _manager.skillPoints = _skillPoints;
        }
    }

    private void LevelUP()
    {
        data.level = ++_level;
        data.nextLevelExp = _nextLevelExp += 100f;
        data.statPoints = _statPoints += 3;
        data.skillPoints = _skillPoints += 1;
    }
}
