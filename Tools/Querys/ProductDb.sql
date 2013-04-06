use ProductDB
go

-- 航班信息缺失
select * from Product
where ProductNo = 'FT-SHA-70083'

select * from Product p
where ProductId in (
	select ItemProductId from Product_Attach_Product
	where ProductId = 643
)
select * from Product_Flight
where ProductId in (672)

select * from Flt_AirLine
where AirLineCode = 'AF'
SELECT * FROM Flt_Airport
where AirportCode in ('SHA', 'cdg', 'bod')

-- BUG
select * from Product
where ProductNo = 'DP-SH-70087'

select * from Picture
where Objectid  =648

--BUG 71
select * from Product
where ProductNo = 'FRN8903'

select * from Product_Schedule
where ProductId = 4 and DayNo = 6
SELECT * FROM Product_Schedule_Detail
where ProductId = 4 and ScheduleId = 35
-- 日程明细不足三项的
select * from Product_Schedule_Detail
where ScheduleId in (
	SELECT ScheduleId FROM Product_Schedule_Detail
	--where ProductId = 4
	group by ProductId, ScheduleId
	having COUNT(1) > 3
)

--BUG 67 单品游机票价格为0
select * from Product
where TripType = 3

select * from Product p
where ProductId in (
	select ItemProductId from Product_Attach_Product
	where ProductId = 782
)
select * from Product_Flight
where ProductId in (665, 666, 667)

select * from Product_Price
where ProductId  in (665, 666, 667)
-- 舱等
select * from Product_Spec
where ProductId  in (793, 794, 795)

--BUG 62 获取签证信息
select * from Product_Info
where InfoTypeName = '签证须知'

--BUG 4 二级目的地的顺序
select * from Property
where SeqNo  is not null

--BUG 69 立减

select * from Product
where IsBackCash = 1

update Product
set IsBackCash = 1
where ProductNo = 'FRN8901'

select * from Picture
where ObjectId = '1'

-- BUG 订购须知
select * from Product_Info
where ProductId = '779'

-- 单品游房型显示问题
select * from Product
where ProductNo = 'FT-SHA-70083'
select * from Product_Price
where ProductId = 782
Order by ProductId, SpecId
--单品游机票显示问题
select * from Product p
where ProductId in (
	select ItemProductId from Product_Attach_Product
	where ProductId = 782
)
select * from Product_Flight
where ProductId in (793, 794, 795)

select * from Product_Price
where ProductId in (793, 794, 795)

UPDATE Product_Price
set EffectDate = '2012-12-01'
WHERE PriceId = 2149

--某目的地/主题下的商品
select * from product_price
where productid in(
select productid from product_property
where propertyid = 48)
--
select * from Property
where ParentPropertyPath LIKE '5%'

select * from product
where productid in(
select productid from product_property
where propertyid = 48)

select * from  product_property pp 
join Property prop on prop.PropertyId = pp.PropertyId
where pp.productid = 779
--

select * from SEO
SELECT * FROM Property 
--where ParentPropertyPath is null
WHERE PropertyName='热门品鉴'

--TASK: 相册分页处理 找一个行程图多的产品

select * from Product
where ProductId in
(
select ObjectId from picture
where ObjectType = '产品' and Location = '行程图'
group by ObjectId, ObjectType, Location
having COUNT(1) > 10
)


select pd.ProductId, COUNT(1) from Product_NoDeparture pd
left join Product p
on pd.ProductId = p.ProductId
group by pd.ProductId
intersect
select pp.ProductId, COUNT(1) from Product_Price pp
left join Product p 
on pp.ProductId = p.ProductId
group by pp.ProductId

select * from Product
where ProductId = 20181
select * from Product_NoDeparture
where ProductId = 20181
select * from Product_Price
where ProductId = 20181

select * from Product_Attach_Product
order by ProductId