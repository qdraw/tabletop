

-- ///////////////// New with random Id /////////////////
--INSERT INTO [ChannelUser] (NameId,Name,NameUrlSafe,IsAccessible,IsVisible)
--VALUES (NEWID(),'Table football','tafelvoetbal','true','true');


-- ///////////////// New Table Footbal user /////////////////
INSERT INTO [dbo].[ChannelUser] (NameId,Name,NameUrlSafe,IsAccessible,IsVisible,Bearer)
VALUES ('06F4D665-AEBF-4704-868F-D052D097C1F5','Table football','tafelvoetbal','true','true','kHZ6ody2nQ9dmcMSCk5m');

-- ///////////////// New Test user on production /////////////////
INSERT INTO [dbo].[ChannelUser] (NameId,Name,NameUrlSafe,IsAccessible,IsVisible,Bearer)
VALUES ('cc9299c5-03a6-409a-be35-30981acfa7ac','Test Channel','test','true','false','kHZ6ody2nQ9dmcMSCk5m');



-- ///////// Update item -- TEST USER /////////////////
UPDATE [ChannelUser]
SET Name = 'Alfred Schmidt', IsVisible= 'false'
WHERE NameId = 'cc9299c5-03a6-409a-be35-30981acfa7ac';


-- /////////////////Update item --tabletop user /////////////////
UPDATE [ChannelUser]
SET Name = 'Alfred Schmidt', IsVisible= 'true'
WHERE NameId = '06F4D665-AEBF-4704-868F-D052D097C1F5';

UPDATE [ChannelUser]
SET Bearer= 'kHZ6ody2nQ9dmcMSCk5m'
WHERE NameId = '06F4D665-AEBF-4704-868F-D052D097C1F5';


-- /////////////////Delete user --tabletop user /////////////////
DELETE [ChannelUser]
WHERE NameId = '4934C95A-2D43-4FDA-9894-C320E002FEA4';

SELECT *  FROM [dbo].[ChannelUser];
