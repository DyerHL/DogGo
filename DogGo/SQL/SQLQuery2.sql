SELECT Id, 
       Name,
       Address,
       Phone,
       Email,
       NeighborhoodId
FROM Owner

SELECT w. Id, w.[Name], ImageUrl, n.[Name]
FROM Walker w
Left Join Neighborhood n on n.Id = w.NeighborhoodId

Select *
From Neighborhood

SELECT o.Id, o.Name, Address, Phone, Email, NeighborhoodId, d.Name
From Owner o
Left Join Dog d on d.OwnerId = o.Id

select * 
from dog
