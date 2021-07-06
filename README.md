# test-project-01

#target

Lets assume we have 3 level hierarchy:
- Provider
- Profile
- Location

Provider 1-many Profile
Provider.ActiveProfileId point to actual version of profile

Profile 1-many Location
each Profile can have 1+ locations

Implement GetItems method

Methods recieve array pairs of { ProviderId, LocationId }
for example [{ providerId = 1, LocationId = null }, { providerId = 2, LocationId = "l-2" }, { providerId = 10, LocationId = "l-not-exsist"  }, { providerId = 5, LocationId = "l-5b" }]
should return objects in same order:

if provideId & locationId exists => return both
if providerId exists but locationId invalid => return provider and 1st location of active Providr Profile & status = LocationNotFound
if providrId not exists => providerId & status = ProvidrNotFound
if providerId exist but activeProfileId invalid => return providerId & status = InvalidProfile