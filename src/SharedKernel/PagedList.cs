namespace SharedKernel;

public class PagedList<T>
{
	private PagedList(
		List<T> items,
		int page,
		int pageSize,
		int totalCount)
	{
		Items = items;
		Page = page;
		PageSize = pageSize;
		TotalCount = totalCount;
	}

	private PagedList(
		List<T> items,
		int totalCount)
	{
		Items = items;
		TotalCount = totalCount;
	}

	public List<T> Items { get; }

	public int Page { get; }

	public int PageSize { get; }

	public int TotalCount { get; }

	public bool HasNextPage => Page * PageSize < TotalCount;

	public bool HasPreviousPage => Page > 1;

	public List<Link> Links { get; set; } = [];

	public static PagedList<T> Create(
		IEnumerable<T> query,
		int page,
		int pageSize)
	{
		int totalCount = query.Count();

		List<T> items = query
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToList();

		return new PagedList<T>(
			items,
			page,
			pageSize,
			totalCount);
	}

	public static PagedList<T> Create(
		IEnumerable<T> query)
	{
        int totalCount = query.Count();

		List<T> items = query
			.ToList();

		return new PagedList<T>(
			items,
			totalCount);
	}

	public static PagedList<T> CreatePaginated(
		IEnumerable<T> query,
		int page,
		int pageSize,
		int totalCount)
	{
		List<T> items = query.ToList();

		return new PagedList<T>(
			items,
			page,
			pageSize,
			totalCount);
	}
}