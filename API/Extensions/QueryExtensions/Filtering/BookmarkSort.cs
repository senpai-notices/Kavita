﻿using System.Linq;
using API.DTOs.Filtering;
using API.Entities;
using API.Extensions.QueryExtensions;

namespace API.Extensions.QueryExtensions.Filtering;

public class BookmarkSeriesPair
{
    public AppUserBookmark Bookmark { get; set; }
    public Series Series { get; set; }
}

public static class BookmarkSort
{
    /// <summary>
    /// Applies the correct sort based on <see cref="SortOptions"/>
    /// </summary>
    /// <param name="query"></param>
    /// <param name="sortOptions"></param>
    /// <returns></returns>
    public static IQueryable<BookmarkSeriesPair> Sort(this IQueryable<BookmarkSeriesPair> query, SortOptions? sortOptions)
    {
        // If no sort options, default to using SortName
        sortOptions ??= new SortOptions()
        {
            IsAscending = true,
            SortField = SortField.SortName
        };

        query = sortOptions.SortField switch
        {
            SortField.SortName => query.DoOrderBy(s => s.Series.SortName.ToLower(), sortOptions),
            SortField.CreatedDate => query.DoOrderBy(s => s.Series.Created, sortOptions),
            SortField.LastModifiedDate => query.DoOrderBy(s => s.Series.LastModified, sortOptions),
            SortField.LastChapterAdded => query.DoOrderBy(s => s.Series.LastChapterAdded, sortOptions),
            SortField.TimeToRead => query.DoOrderBy(s => s.Series.AvgHoursToRead, sortOptions),
            SortField.ReleaseYear => query.DoOrderBy(s => s.Series.Metadata.ReleaseYear, sortOptions),
            SortField.ReadProgress => query.DoOrderBy(s => s.Series.Progress.Where(p => p.SeriesId == s.Series.Id).Select(p => p.LastModified).Max(), sortOptions),
            _ => query
        };

        return query;
    }
}
