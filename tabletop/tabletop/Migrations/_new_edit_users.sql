

-- ///////////////// New with random Id /////////////////
--INSERT INTO [ChannelUser] (NameId,Name,NameUrlSafe,IsAccessible,IsVisible)
--VALUES (NEWID(),'Table football','tafelvoetbal','true','true');


-- ///////////////// New Table Footbal user /////////////////
INSERT INTO [dbo].[ChannelUser] (NameId,Name,NameUrlSafe,IsAccessible,IsVisible)
VALUES ('06F4D665-AEBF-4704-868F-D052D097C1F5','Table football','tafelvoetbal','true','true');

-- ///////////////// New Test user on production /////////////////
INSERT INTO [dbo].[ChannelUser] (NameId,Name,NameUrlSafe,IsAccessible,IsVisible)
VALUES ('cc9299c5-03a6-409a-be35-30981acfa7ac','Test Channel','test','true','false');


-- ///////// Update item -- TEST USER /////////////////
UPDATE [ChannelUser]
SET Name = 'Alfred Schmidt', IsVisible= 'false'
WHERE NameId = 'cc9299c5-03a6-409a-be35-30981acfa7ac';


-- /////////////////Update item --tabletop user /////////////////
UPDATE [ChannelUser]
SET Name = 'Alfred Schmidt', IsVisible= 'true'
WHERE NameId = '06F4D665-AEBF-4704-868F-D052D097C1F5';

SELECT *  FROM [dbo].[ChannelUser];
